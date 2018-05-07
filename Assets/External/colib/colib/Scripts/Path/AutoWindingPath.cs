using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using CoLib.Utils;

namespace CoLib
{

/// <summary>
/// Automatically winds the rotation of a path, based on a starting up vector.
/// </summary>
public class AutoWindingPath : IPath 
{
	#region Public properties

	public Vector3 StartUpVector
	{
		get { return _correctedStartUp; }
	}

	public Vector3 EndUpVector
	{
		get { return GetNormal(1.0); }
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Creates an autowinding path, which befores rotation pre-sample calculations.
	/// </summary>
	/// <param name="path">A subpath, to autowind.</param>
	/// <param name="startUp">The up vector at the start of the path.</param>
	public AutoWindingPath(IPath path, Vector3 startUp)
	{
		_childPath = path;
		CalculateSamples(startUp, Vector3.zero);
	}

	/// <summary>
	/// Creates an autowinding path, which befores rotation pre-sample calculations.
	/// </summary>
	/// <param name="path">A subpath, to autowind.</param>
	/// <param name="startUp">The up vector at the start of the path.</param>
	/// <param name="endUp"> The up vector at the end of the path. </param>
	public AutoWindingPath(IPath path, Vector3 startUp, Vector3 endUp)
	{
		_childPath = path;
		CalculateSamples(startUp, endUp);
	}

	/// <summary>
	/// Creates an autowinding path, which befores rotation pre-sample calculations.
	/// </summary>
	/// <param name="path">A subpath, to autowind.</param>
	/// <param name="startUp">The up vector at the start of the path.</param>
	/// <param name="endUp"> The up vector at the end of the path. </param>
	public AutoWindingPath(IPath path, Vector3 startUp, Vector3 endUp, float parallelAngle)
	{
		_childPath = path;
		_parallelAngle = Mathf.Abs(parallelAngle);
		CalculateSamples(startUp, endUp);
	}

	#endregion

	#region IPath

	public float Length 
	{ 
		get 
		{
			return _childPath.Length;
		} 
	}

	public Vector3 GetPoint(double t)
	{
		return _childPath.GetPoint(t);	
	}

	public Vector3 GetTangent(double t)
	{
		return _childPath.GetTangent(t);
	}

	public Vector3 GetNormal(double t)
	{
		Vector3 up = GetUpVector(t);
		return _childPath.GetNormal(t, up);
	}

	public Vector3 GetNormal(double t, Vector3 up)
	{
		double startT, endT;

		FindParallelSegmentTimes(t, _parallelAngle, up, out startT, out endT);

		Vector3 startUp, endUp;

		if (startT == 0.0 && !HasAcceptableAngle(startT, _parallelAngle, up) ) { 
			startUp = GetUpVector(startT).normalized;
		} else {
			startUp = _childPath.GetNormal(startT, up);
		}

		if (endT == 1.0 && !HasAcceptableAngle(endT, _parallelAngle, up)) {
			endUp = GetUpVector(endT).normalized;
		} else {
			endUp = _childPath.GetNormal(endT, up);
		}

		Vector3 startTangent = GetTangent(startT);
		var startMidUp = Quaternion.Inverse(Quaternion.LookRotation(startTangent, GetNormal(startT))) * Quaternion.LookRotation(startTangent, startUp);

		Vector3 endTangent = GetTangent(endT);
		var endMidUp = Quaternion.Inverse(Quaternion.LookRotation(endTangent, GetNormal(endT))) * Quaternion.LookRotation(endTangent, endUp);

		float normalisedT = Mathf.InverseLerp((float) startT, (float) endT, (float) t);

		Quaternion rot = QuaternionUtils.SlerpNoInvert(startMidUp, endMidUp, normalisedT);

		Vector3 tangent = GetTangent(endT);
		Quaternion baseRot = Quaternion.LookRotation(tangent, GetNormal(t));

		Vector3 interimUp = (baseRot * rot * Vector3.up).normalized;

		return _childPath.GetNormal(t, interimUp);
	}

	public Vector3 GetAcceleration(double t)
	{
		return _childPath.GetAcceleration(t);
	}

	public Quaternion GetRotation(double t)
	{
		Vector3 normal = GetNormal(t);

		return _childPath.GetRotation(t, normal);
	}

	public Quaternion GetRotation(double t, Vector3 up)
	{
		Vector3 normal = GetNormal(t, up);
		return _childPath.GetRotation(t, normal);
	}

	public double GetUnmappedTime(double t)
	{
		return _childPath.GetUnmappedTime(t);
	}

	public double GetMappedTime(double t)
	{
		return _childPath.GetMappedTime(t);
	}

	public IEnumerable<double> GetCriticalPoints() { return _childPath.GetCriticalPoints(); }

	#endregion

	#region Private methods

	/// <summary>
	/// Find an up vector by interpolating the two nearest presampled up vectors.
	/// </summary>
	/// <returns>The up vector.</returns>
	/// <param name="t">The time to look for the up vector.</param>
	private Vector3 GetUpVector(double t)
	{
		return (GetRotationQuat(t) * _correctedStartUp).normalized;
	}

	private Quaternion GetRotationQuat(double t)
	{
		t = GetMappedTime(t);
		// Find the start/end of the sample interval.
		int index = GetIndex(t);

		Sample sample = _samples[index];

		// Lerp between the samples
		double progress = (t - sample.StartTime) / (sample.EndTime - sample.StartTime);

		if (index > 0 && index < _samples.Count - 1) {
			Sample prevUp = _samples[index - 1];
			Sample nextUp = _samples[index + 1];
			return QuaternionUtils.Squad(prevUp.StartRotation, sample.StartRotation, sample.EndRotation, nextUp.EndRotation, (float) progress);
		} else {
			return QuaternionUtils.SlerpNoInvert(sample.StartRotation, sample.EndRotation, (float) progress);
		}
	}

	/// <summary>
	/// Tries to find a segment of the timeline which is close to being parallel to a global up vector. 
	/// </summary>
	/// <param name="t">The time to check around.</param>
	/// <param name="angle">
	/// The maximum angle the paths tangent should have from the global up vector, before being considered parallel.
	/// </param>
	/// <param name="globalUp">A global up vector</param>
	/// <param name="startT">Outputs the start time of the segment, (this can be at minimum 0).</param>
	/// <param name="endT">Outputs the end time of the segment, (this can be a maximum of 1). </param>
	private void FindParallelSegmentTimes(double t, float angle, Vector3 globalUp, out double startT, out double endT)
	{
		// Test outwards on expanding path, looking for a flat segment.
		startT = t; endT = t;

		bool anglesAcceptable = HasAcceptableAngle(t, angle, globalUp);

		// Search backwards at fixed sample positions for a point on the path that isn't parallel to the global up.
		int index = GetIndex(t);
		for (int startIndex = index; startIndex >= 0; --startIndex) {
			startT = _samples[startIndex].StartTime;
			if (HasAcceptableAngle(startT, angle, globalUp)) { break; }
			anglesAcceptable = false;
		}

		// Search forwards at fixed sample positions for a point on the path that isn't parallel to the global up.
		for (int endIndex = index; endIndex < _samples.Count ; ++endIndex) {
			endT = _samples[endIndex].EndTime;
			if (HasAcceptableAngle(endT, angle, globalUp)) { break; } 
			anglesAcceptable = false;
		}

		if (anglesAcceptable) {
			startT = t;
			endT = t;
		}		

		startT = Mathf.Clamp01((float) startT);
		endT = Mathf.Clamp01((float)endT);
	}

	private void CalculateSamples(Vector3 startUp, Vector3 endUp)
	{
		const int SubIterCount = 8;

		_samples = CreateSamples();

		Vector3 startTangent = GetTangent(GetUnmappedTime(0.0));
		_correctedStartUp = Vector3.Cross(startTangent, Vector3.Cross(startUp, startTangent)).normalized;

		Vector3 lastTangent = startTangent;

		// Forward pass, calculate the change in rotation by iterating through path at set sample points.
		Quaternion lastQuat = Quaternion.identity;
		for (int i = 0; i < _samples.Count; ++i) {
			Sample sample = _samples[i];
			double startTime = i == 0? 0.0 : _samples[i - 1].StartTime;
			double endTime = _samples[i].StartTime;
			for (int j = 0; j < SubIterCount; ++j) {
				double t = (j / (double) SubIterCount) * (endTime - startTime) + startTime;
				Vector3 currentTangent = _childPath.GetTangent(GetUnmappedTime(t));
				// Calculate the rotation change between the last tangent and the current one. 
				lastQuat = lastQuat * QuaternionUtils.FromToRotation(lastTangent, currentTangent);

				lastTangent = currentTangent;
			}
			sample.StartRotation = lastQuat;
			_samples[i] = sample;
		}

		// Check if there is a predefined end up vector to work towards.
		if (endUp.magnitude != 0f) { 
			// Recalcute the end up vector so that it is tangential.
			Vector3 endTangent = GetTangent(GetUnmappedTime(1.0));
			var correctedEndUp = Vector3.Cross(endTangent, Vector3.Cross(endUp, endTangent)).normalized;

			// Find the rotation which get's the final sample rotation to align with our correct up vector;
			lastQuat = QuaternionUtils.FromToRotation( _samples[_samples.Count - 1].StartRotation * _correctedStartUp, correctedEndUp);

			for (int i = _samples.Count - 1; i >= 0; --i) {
				Sample sample = _samples[i];
				double t = sample.StartTime;

				// Apply the correction rotation gradually across the path.
				sample.StartRotation = QuaternionUtils.SlerpNoInvert(_samples[i].StartRotation, lastQuat * _samples[i].StartRotation, (float) t);
				_samples[i] = sample;
			}
		}

		// Update the endpoints
		for (int i = 0; i < _samples.Count - 1; ++i) {
			int nextIndex = i + 1;
			var sample = _samples[i];
			sample.EndTime = _samples[nextIndex].StartTime;
			sample.EndRotation = _samples[nextIndex].StartRotation;
			_samples[i] = sample;
		}
		_samples.RemoveAt(_samples.Count - 1);
	}

	private List<Sample> CreateSamples()
	{
		// Creates a list of all the sample times. We combine the critical points with regular sample intervals.
		List<double> samplePoints =  _childPath.GetCriticalPoints().Select(GetMappedTime).ToList();

		// We add a bunch of sample points between adjacent critical points.
		List<double> extraSamplePoints = new List<double>();

		for (int i = 0; i < samplePoints.Count - 1; ++i) {
			double extraPoint = (samplePoints[i] + samplePoints[i + 1]) / 2.0;
			extraSamplePoints.Add(extraPoint);
		}
		samplePoints.AddRange(extraSamplePoints);
		samplePoints.Sort();

		// Create a list of samples, with the given times.
		return samplePoints
			.Distinct()
			.Select( t => new Sample() { StartTime = t, StartRotation = Quaternion.identity })
			.ToList();
	}

	private int GetIndex(double t)
	{
		// Find the sample that contains our time.
		int index = _samples.BinarySearchStruct( (item) => {
			if (item.StartTime > t) { return -1; }
			if (item.EndTime <= t) { return 1; }
			return 0;
		});
		if (index >= 0 && index < _samples.Count) { return index; }
		return t <= 0.0 ? 0 : _samples.Count - 1;
	}

	private float GetAngle(double t, float angle, Vector3 globalUp)
	{
		Vector3 tangent = _childPath.GetTangent(t);
		return Vector3.Angle(tangent, globalUp);
	}

	private bool HasAcceptableAngle(double t, float angle, Vector3 globalUp)
	{
		Vector3 tangent = _childPath.GetTangent(t);
		float curAngle = Mathf.Min( Mathf.Abs(Vector3.Angle(tangent, globalUp)), Mathf.Abs(Vector3.Angle(tangent, -globalUp)));
		return (curAngle > angle);
	}

	#endregion

	#region Private fields

	private struct Sample
	{
		public double StartTime;
		public double EndTime;
		public Quaternion StartRotation;
		public Quaternion EndRotation;
	}

	private readonly float _parallelAngle = 45f;

	private IPath _childPath;
	private List<Sample> _samples;
	private Vector3 _correctedStartUp;

	#endregion
}

}
