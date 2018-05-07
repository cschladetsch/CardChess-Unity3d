using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace CoLib
{

/// <summary>
/// Class which automatically generates the control points of a bezier, given a series of points to traverse through.
/// </summary>
public class AutoBezierPath: IPath
{
	#region Public methods

	/// <summary>
	/// Creates an automatic bezier path between the given list of points. This path will be a catmull-rom spline.
	/// </summary>
	/// <param name="points">The points the bezier should map through.</param>
	public AutoBezierPath(params Vector3[] points):
		this(0f, 0f, 0f, false, PathOvershootMode.Clamped, points as IEnumerable<Vector3>)
	{
	}

	/// <summary>
	/// Creates an automatic bezier path between the given list of points. This path will be a catmull-rom spline.
	/// </summary>
	/// <param name="points">The points the bezier should map through.</param>
	public AutoBezierPath(IEnumerable<Vector3> points):
		this(0f, 0f, 0f, false, PathOvershootMode.Clamped, points)
	{
	}

	/// <summary>
	/// Creates an automatic bezier path between the given list of points. This path will be a catmull-rom spline.
	/// </summary>
	/// <param name="looped">
	/// Whether the path should be looped or not.
	/// </param>
	/// <param name="overshootMode">
	/// How path overshooting should be handled.
	/// </param>
	/// <param name="points">The points the bezier should map through.</param>
	public AutoBezierPath(bool looped, PathOvershootMode overshootMode, params Vector3[] points):
		this(0f, 0f, 0f, looped, overshootMode, points as IEnumerable<Vector3>)
	{
	}

	/// <summary>
	/// Creates an automatic bezier path between the given list of points. This path will be a catmull-rom spline.
	/// </summary>
	/// <param name="looped">
	/// Whether the path should be looped or not.
	/// </param>
	/// <param name="overshootMode">
	/// How path overshooting should be handled.
	/// </param>
	/// <param name="points">The points the bezier should map through.</param>
	public AutoBezierPath(bool looped, PathOvershootMode overshootMode, IEnumerable<Vector3> points):
		this(0f, 0f, 0f, looped, overshootMode, points)
	{
	}

	/// <summary>
	/// Creates an automatic bezier path between the given list of points.  If tension, bias and continuity are all 0,
	/// This path will be a catmull-rom spline.
	/// </summary>
	/// <param name="tension">
	/// A tension of 0 indicates smooth corners. A tension of 1 indicates sharp corners. Values larger than 
	/// this range can cause the curves to overshoot. Values smaller than 0 will cause the curves to fold into 
	/// themselves.
	/// </param>
	/// <summary>
	/// Bias affects how automatic control points should be generated. For a given knot, a positive bias means there 
	/// will be a strong bend at the start of the outgoing segment, and a negative bias means there will be a strong 
	/// bend at the end of the incomming segment. The default bias of 0 gives a neutral weighting.
	/// </summary>
	/// <summary>
	/// Continuity affects how automatic control points should be generated. If this value is 0, control points will
	/// be C1 continious, (no instant changes in velocity, unless tension is >= 1). This can be though to change how
	/// sharp the shape is.
	/// </summary>
	/// <param name="looped">
	/// Whether the path should be looped or not.
	/// </param>
	/// <param name="overshootMode">
	/// How path overshooting should be handled.
	/// </param>
	/// <param name="points">The points the bezier should map through.</param>
	public AutoBezierPath(float tension, float bias, float continuity, bool looped, PathOvershootMode overshootMode, params Vector3[] points): 
		this(tension, bias, continuity, looped, overshootMode, points as IEnumerable<Vector3>)
	{
	}

	/// <summary>
	/// Creates an automatic bezier path between the given list of points. If tension, bias and continuity are all 0,
	/// This path will be a catmull-rom spline.
	/// </summary>
	/// <param name="tension">
	/// A tension of 0 indicates smooth corners. A tension of 1 indicates sharp corners. Values larger than 
	/// this range can cause the curves to overshoot. Values smaller than 0 will cause the curves to fold into 
	/// themselves.
	/// </param>
	/// <summary>
	/// Bias affects how automatic control points should be generated. For a given knot, a positive bias means there 
	/// will be a strong bend at the start of the outgoing segment, and a negative bias means there will be a strong 
	/// bend at the end of the incomming segment. The default bias of 0 gives a neutral weighting.
	/// </summary>
	/// <summary>
	/// Continuity affects how automatic control points should be generated. If this value is 0, control points will
	/// be C1 continious, (no instant changes in velocity, unless tension is >= 1). This can be though to change how
	/// sharp the shape is.
	/// </summary>
	/// <param name="looped">
	/// Whether the path should be looped or not.
	/// </param>
	/// <param name="overshootMode">
	/// How path overshooting should be handled.
	/// </param>
	/// <param name="points">The points the bezier should map through.</param>
	public AutoBezierPath(float tension, float bias, float continuity, bool looped, PathOvershootMode overshootMode, 
		IEnumerable<Vector3> points)
	{
		List<CubicBezierKnot> knots = CubicBezierKnotUtils.GeneratePathKnots(looped, points, tension, bias, continuity);
		var paths = new List<IPath>();

		int numberOfPoints = points.Count();
		int count = looped ? numberOfPoints : numberOfPoints - 1;

		for (int i = 0; i < count; ++i) {
			int index = i;
			int nextIndex = (i + 1) % numberOfPoints;
			CubicBezierKnot firstKnot = knots[index];
			CubicBezierKnot nextKnot = knots[nextIndex];

			var path = new NormalizedBezierPath(firstKnot.Position, firstKnot.OutControlPoint, nextKnot.InControlPoint,
				nextKnot.Position);
			paths.Add(path);
		}
		_path = new ChainedPath(overshootMode, paths);
	}

	#endregion

	#region IPath

	public float Length { get { return _path.Length; } }

	public Vector3 GetPoint(double t) { return _path.GetPoint(t); }

	public Vector3 GetTangent(double t) { return _path.GetTangent(t); }

	public Vector3 GetNormal(double t) { return _path.GetNormal(t); }

	public Vector3 GetNormal(double t, Vector3 up) { return _path.GetNormal(t, up); }

	public Vector3 GetAcceleration(double t) { return _path.GetAcceleration(t); }

	public Quaternion GetRotation(double t) { return _path.GetRotation(t); }

	public Quaternion GetRotation(double t, Vector3 up) { return _path.GetRotation(t, up); }

	public IEnumerable<double> GetCriticalPoints() { return _path.GetCriticalPoints(); }

	public double GetUnmappedTime(double t) { return _path.GetUnmappedTime(t); } 
	public double GetMappedTime(double t) { return _path.GetMappedTime(t); }


	#endregion

	#region Private fields

	private readonly ChainedPath _path;

	#endregion
}

}
