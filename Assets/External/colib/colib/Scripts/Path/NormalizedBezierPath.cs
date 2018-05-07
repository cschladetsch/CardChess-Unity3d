using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using CoLib.Utils;

namespace CoLib
{

/// <summary>
/// A path which connects two points.
/// </summary>
public class NormalizedBezierPath: IPath
{
	#region Public methods

	/// <summary>
	/// Creates a quadratic bezier path connecting two points.
	/// </summary>
	/// <param name="startPoint">The start of the path.</param>
	/// <param name="controlPoint">The control point.</param>
	/// <param name="endPoint">The end of the path.</param>
	public NormalizedBezierPath(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint)
	{
		_segment = new CubicBezierSegment(startPoint, controlPoint, endPoint);
		_coefficients = _segment.CalculateTangentCoefficients();

		CalculateSamples();
	}

	/// <summary>
	/// Creates a cubic bezier path connecting two points.
	/// </summary>
	/// <param name="startPoint">The start of the path.</param>
	/// <param name="startControlPoint">The first control point.</param>
	/// <param name="startControlPoint">The second control point.</param>
	/// <param name="endPoint">The end of the path.</param>
	public NormalizedBezierPath(Vector3 startPoint, Vector3 startControlPoint, Vector3 endControlPoint, Vector3 endPoint)
	{
		_segment = new CubicBezierSegment(startPoint, startControlPoint, endControlPoint, endPoint);
		_coefficients = _segment.CalculateTangentCoefficients();
		CalculateSamples();
	}

	#endregion

	#region IPath

	/// <summary>
	/// An approximate length of the bezier curve.
	/// </summary>
	/// <value>The length.</value>
	public float Length 
	{ 
		get 
		{
			return _length; 
		} 
	}

	public Vector3 GetPoint(double t)
	{
		t = NormalizeTime(t);
		return _segment.GetPoint(t);	
	}

	public Vector3 GetTangent(double t)
	{
		t = NormalizeTime(t);
		return _coefficients.GetTangent(t);
	}

	public Vector3 GetNormal(double t)
	{
		t = NormalizeTime(t);
		return _segment.GetNormal(_coefficients, t);
	}

	public Vector3 GetNormal(double t, Vector3 up)
	{
		t = NormalizeTime(t);
		return _segment.GetNormal(_coefficients, t, up);
	}

	public Vector3 GetAcceleration(double t)
	{
		t = NormalizeTime(t);
		return _coefficients.GetAcceleration(t);
	}

	public Quaternion GetRotation(double t)
	{	
		t = NormalizeTime(t);

		return Quaternion.LookRotation(_coefficients.GetTangent(t), _segment.GetNormal(_coefficients, t, Vector3.up));
	}

	public Quaternion GetRotation(double t, Vector3 up)
	{
		t = NormalizeTime(t);
		return Quaternion.LookRotation(_coefficients.GetTangent(t), _segment.GetNormal(_coefficients, t, up));
	}

	public IEnumerable<double> GetCriticalPoints() 
	{
		var times = _coefficients.GetPointsOfInflectionTimes()
			.Select( t => GetUnmappedTime(t))
			.ToList();
		times.Insert(0, 0.0);
		times.Insert(times.Count, 1.0);
		return times.Distinct(); 
	}

	public double GetUnmappedTime(double t)
	{
		return DenormalizeTime(t);
	} 

	public double GetMappedTime(double t)
	{
		return NormalizeTime(t);
	}

	#endregion

	#region Private methods

	private double NormalizeTime(double t)
	{
		// This operation takes log(n) time, where n is the constant NumberOfSamples.
		if (t <= 0.0) { return t; }
		if (t >= 1.0) { return t; }

		// Find the sample that contains our time.
		int index = _samples.BinarySearchStruct( (item) => {

			if (item.StartTime > t) { return -1; }
			if (item.EndTime <= t) { return 1; }
			return 0;
		});

		SegmentSample sample = _samples[index];
		double segmentT = (t - sample.StartTime) / (sample.EndTime - sample.StartTime);
		// Each segment is spaced at 1/NumberOfSegments intervals along our curve.
		return (index + segmentT) / (double) NumberOfSamples;
	}

	private double DenormalizeTime(double t)
	{
		// This operation takes log(n) time, where n is the constant NumberOfSamples.
		if (t <= 0.0) { return t; }
		if (t >= 1.0) { return t; }

		double total = t * NumberOfSamples;
		int index = Mathf.FloorToInt((float) total);
		double segmentT = total - index;

		SegmentSample sample = _samples[index];
		return (segmentT * (sample.EndTime - sample.StartTime)) + sample.StartTime;
	}

	private void CalculateSamples()
	{
		var workingSegment = new CubicBezierSegment();
		workingSegment.StartPoint = _segment.StartPoint;
		workingSegment.StartControlPoint = _segment.StartControlPoint;
		workingSegment.EndControlPoint = _segment.EndControlPoint;
		workingSegment.EndPoint = _segment.EndPoint;

		_length = 0.0f;

		// First, break the working segment down into a discrete number of segments. We only care about the approximate
		// length of these segments, not the curve segments themselves.
		CubicBezierSegment leftSegment = new CubicBezierSegment(), rightSegment = new CubicBezierSegment();

		for (int i = 0; i < NumberOfSamples; ++i) {
			// Subdivide another part off the curve. We take 1/64 of the curve on the first loop, and keep the 
			// remaining 63/64 of the curve. On the next loop, we want 1/63 of the curve and so on, to keep the
			// segment size consistent.
			double splitT = 1 / (double)(NumberOfSamples - i); 
			workingSegment.Subdivide(leftSegment, rightSegment, splitT);

			float nodeLength = leftSegment.CalculateLength();
			_length += nodeLength;
			SegmentSample sample = new SegmentSample();
			sample.Length = nodeLength;
			sample.StartTime = 0.0f;
			sample.EndTime = 1.0f;
			_samples.Add(sample);

			// Copy the right curve segment into the working curve segment.
			workingSegment.StartPoint = rightSegment.StartPoint;
			workingSegment.StartControlPoint = rightSegment.StartControlPoint;
			workingSegment.EndControlPoint = rightSegment.EndControlPoint;
			workingSegment.EndPoint = rightSegment.EndPoint;
		}

		// Knowing what the length of the curve segments are, and a total approximate length, we can calculate the
		// the start/end times of each segment. These will be the values we use for our runtime lookup.
		float runningLength = _samples[0].Length;

		for (int i = 1; i < _samples.Count; ++i) {
			float t = (float) runningLength / _length;

			SegmentSample sample = _samples[i];
			sample.StartTime = t;
			_samples[i] = sample;

			SegmentSample oldSample = _samples[i - 1];
			oldSample.EndTime = t;
			_samples[i - 1] = oldSample;

			runningLength += sample.Length;
		}
	}

	#endregion

	#region Private fields

	private struct SegmentSample
	{
		public float StartTime;
		public float EndTime;
		public float Length;
	}

	private const int NumberOfSamples = 32;

	private readonly CubicBezierSegment _segment;
	private readonly CubicBezierTangentCoefficients _coefficients;
	private readonly List<SegmentSample> _samples = new List<SegmentSample>(NumberOfSamples);
	private float _length;

	#endregion
}

}
