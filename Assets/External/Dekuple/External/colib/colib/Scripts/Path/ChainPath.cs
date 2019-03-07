using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

using CoLib.Utils;

namespace CoLib
{

[Serializable]
public enum PathOvershootMode
{
	/// <summary>
	/// Wrapped paths immediately repeat the start of the path when overshooting
	/// </summary>
	Wrapped,
	/// <summary>
	/// Clamped paths clamp t to the range [0,1].
	/// </summary>
	Clamped,
	/// <summary>
	/// Extrapolate paths use child path extrapolation to determine how to handle values outside the range [0,1].
	/// </summary>
	Extrapolate
}

/// <summary>
/// A path which changes together several paths.
/// </summary>
public class ChainedPath: IPath 
{
	#region Public methods

	/// <summary>
	/// Construct a new path, using child paths. This path will use the Extrapolate overshoot mode.
	/// </summary>
	/// <param name="paths">The child paths to chain together.</param>
	public ChainedPath(params IPath[] paths) : this(PathOvershootMode.Extrapolate, paths)
	{
	}

	/// <summary>
	/// Construct a new path, using child paths. This path will use the Extrapolate overshoot mode.
	/// </summary>
	/// <param name="paths">The child paths to chain together.</param>
	public ChainedPath(IEnumerable<IPath> paths) : this(PathOvershootMode.Extrapolate, paths as IEnumerable<IPath>)
	{
	}

	/// <summary>
	/// Construct a new path, using child paths.
	/// </summary>
	/// <param name="wrappedOvershoot">	Determines over/undershooting behaviour. </param>
	/// <param name="paths">The child paths to chain together.</param>
	public ChainedPath(PathOvershootMode overshootMode, params IPath[] paths): 
		this(overshootMode, paths as IEnumerable<IPath>)
	{
	}

	public ChainedPath(PathOvershootMode overshootMode, IEnumerable<IPath> paths)
	{
		_segments = new List<Segment>();
		_overshootMode = overshootMode;

		float length = 0f;
		int pathCount = 0;
		foreach (var path in paths) {
			pathCount++;
			length += path.Length;
		}
		if (pathCount == 0) { return; }

		_length = length;
		double t = 0.0;

		if (length <= 0.0)  {
			Segment segment = new Segment();
			segment.StartTime = 0;
			segment.EndTime = 1.0;
			segment.Path = paths.First();
			_segments.Add(segment);
			return;
		}

		foreach (var path in paths) {
			Segment segment = new Segment();
			segment.Path = path;
			segment.StartTime = t;
			t += path.Length / _length; 
			segment.EndTime = t;
			_segments.Add(segment);
		}
	}

	#endregion

	#region IPath

	public float Length { get; private set; }

	public Vector3 GetPoint(double t)
	{
		if (_segments.Count == 0) { return Vector3.zero; }
		t = RenormalizeTime(t);
		Segment segment = GetSegment(t);

		double normalizedT = GetSegmentTime(segment, t);

		// Use lerp unclamped to allow overshoot.
		return segment.Path.GetPoint(normalizedT);
	}

	public Vector3 GetTangent(double t)
	{
		if (_segments.Count == 0) { return Vector3.forward; }
		t = RenormalizeTime(t);
		Segment segment = GetSegment(t);
		double normalizedT = GetSegmentTime(segment, t);

		// Use lerp unclamped to allow overshoot.
		return segment.Path.GetTangent(normalizedT);
	}

	public Vector3 GetNormal(double t)
	{
		if (_segments.Count == 0) { return Vector3.up; }
		t = RenormalizeTime(t);
		Segment segment = GetSegment(t);
		double normalizedT = GetSegmentTime(segment, t);	
		return segment.Path.GetNormal(normalizedT);
	}

	public Vector3 GetNormal(double t, Vector3 up)
	{
		if (_segments.Count == 0) { return up; }
		t = RenormalizeTime(t);
		Segment segment = GetSegment(t);
		double normalizedT = GetSegmentTime(segment, t);	
		return segment.Path.GetNormal(normalizedT, up);
	}

	public Vector3 GetAcceleration(double t)
	{
		if (_segments.Count == 0) { return Vector3.zero; }
		t = RenormalizeTime(t);
		Segment segment = GetSegment(t);
		double normalizedT = GetSegmentTime(segment, t);	
		return segment.Path.GetNormal(normalizedT);
	}

	public Quaternion GetRotation(double t)
	{
		if (_segments.Count == 0) { return Quaternion.identity; }

		t = RenormalizeTime(t);
		Segment segment = GetSegment(t);
		double normalizedT = GetSegmentTime(segment, t);

		Vector3 tangent = segment.Path.GetTangent(normalizedT);
		Vector3 normal = segment.Path.GetNormal(normalizedT);

		return Quaternion.LookRotation(tangent, normal);
	}

	public Quaternion GetRotation(double t, Vector3 up)
	{
		if (_segments.Count == 0) { return Quaternion.identity; }

		t = RenormalizeTime(t);
		Segment segment = GetSegment(t);
		double normalizedT = GetSegmentTime(segment, t);

		Vector3 tangent = segment.Path.GetTangent(normalizedT);
		Vector3 normal = segment.Path.GetNormal(normalizedT, up);

		return Quaternion.LookRotation(tangent, normal);
	}

	public IEnumerable<double> GetCriticalPoints() 
	{
		return _segments
			.SelectMany( segment => 
				segment.Path.GetCriticalPoints()
					.Select(t => t * (segment.EndTime - segment.StartTime) + segment.StartTime)
			)
			.Distinct();
	}
	 
	public double GetUnmappedTime(double t)
	{
		Segment segment = GetSegment(t);
		double segmentTime = GetSegmentTime(segment, t);
		double unmappedTime = segment.Path.GetUnmappedTime(segmentTime);
		return unmappedTime * (segment.EndTime - segment.StartTime) + segment.StartTime;
	}

	public double GetMappedTime(double t) 
	{ 
		Segment segment = GetSegment(t);
		double segmentTime = GetSegmentTime(segment, t);
		double unmappedTime = segment.Path.GetMappedTime(segmentTime);
		return unmappedTime * (segment.EndTime - segment.StartTime) + segment.StartTime;
	}

	#endregion

	#region Private methods

	private Segment GetSegment(double t)
	{
		int index = 0;
		if (t < 0) { 
			index = 0;
		} else if (t > 1) { 
			index = _segments.Count - 1;
		} else {
			index = _segments.BinarySearchStruct( (item) => {
				if (item.StartTime > t) { return -1; }
				if (item.EndTime <= t) { return 1; }
				return 0;
			});
		}
		if (index < 0) { 
			index = _segments.Count - 1;
		}
		return _segments[index];
	}

	private double GetSegmentTime(Segment segment, double t)
	{
		double segmentDuration = (segment.EndTime - segment.StartTime);
		if (segmentDuration <= 0.0) { return 1.0; }
		return (t - segment.StartTime) / segmentDuration;
	}

	private double RenormalizeTime(double t)
	{
		switch (_overshootMode) {
			case PathOvershootMode.Clamped:
				t = t < 0.0 ? 0.0 : (t < 1.0 ? t : 1.0);
				break;
			case PathOvershootMode.Wrapped:
				// Modulus operator doesn't work correctly with negative floats.
				t = t - Math.Truncate(t);
				t = t < 0 ? 1 + t : t;
				break;
		}
		return t;
	}


	#endregion

	#region Private fields

	private struct Segment
	{
		public IPath Path;
		public double StartTime;
		public double EndTime;
	}

	private readonly List<Segment> _segments;
	private readonly float _length;
	private readonly PathOvershootMode _overshootMode;

	#endregion
}

}
