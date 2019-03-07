using System;
using UnityEngine;

namespace CoLib
{

[Serializable]
public enum CubicBezierKnotType
{
	/// <summary>
	/// Indicates C2 continuous, (so tangents and accelerations are aligned at this knot). This means the control points
	/// are perfectly mirrored around the knot position.
	/// </summary>
	Continuous,
	/// <summary>
	/// Indicated C1 continous, (so only tangents are aligned at this knot). This means that the direction to the control
	/// points is mirrrored, but the magnitude of their offsets can be different.
	/// </summary>
	Aligned,
	/// <summary>
	/// The control points are both exactly at the knot.
	/// </summary>
	Singular,
	/// <summary>
	/// The controls points can be anything.
	/// </summary>
	Broken
}

[Serializable]
/// <summary>
/// Represents a vertex in a bezier spline path. This class is used for displaying data in the editor. Note that the
/// CubicBezierKnotType won't modify InControlPoint or OutControlPoint unless 
/// CubicBezierKnotUtils.RecalculateControlPoint is called. 
/// </summary>
public class CubicBezierKnot
{
	public CubicBezierKnotType Type;
	public Vector3 Position;
	public Vector3 Up;
	public Vector3 InControlPoint;
	public Vector3 OutControlPoint;
}

}
