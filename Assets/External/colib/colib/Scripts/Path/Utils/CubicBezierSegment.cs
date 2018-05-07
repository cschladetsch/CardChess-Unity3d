using System;
using UnityEngine;

namespace CoLib
{

[Serializable]
/// <summary>
/// Describes a cubic bezier curve.
/// </summary>
public class CubicBezierSegment
{
	#region Public properties

	public Vector3 StartPoint;
	public Vector3 StartControlPoint;
	public Vector3 EndControlPoint;
	public Vector3 EndPoint;

	#endregion

	#region Public methods

	public CubicBezierSegment()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CoLib.CubicBezierSegment"/> class. This constructor takes three 
	/// points, (which describe a QuadraticBezierCurve), and automatically converts them into a CubicBezierCurve.
	/// </summary>
	/// <param name="startPoint">The start point of the curve.</param>
	/// <param name="controlPoint">The quadratic control point of the curve.</param>
	/// <param name="endPoint">The end point of the curve.</param>
	public CubicBezierSegment(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint)
	{
		StartPoint = startPoint;
		StartControlPoint = (startPoint + controlPoint * 2f) / 3f;
		EndControlPoint = (endPoint + controlPoint * 2f) / 3f;
		EndPoint = endPoint;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CoLib.CubicBezierSegment"/> class.
	/// </summary>
	/// <param name="startPoint">The start point of the curve.</param>
	/// <param name="startControlPoint">The first control point of the curve.</param>
	/// <param name="startControlPoint">The second control point of the curve.</param>
	/// <param name="endPoint">The end point of the curve.</param>
	public CubicBezierSegment(Vector3 startPoint, Vector3 startControlPoint, Vector3 endControlPoint, Vector3 endPoint)
	{
		StartPoint = startPoint;
		StartControlPoint = startControlPoint;
		EndControlPoint = endControlPoint;
		EndPoint = endPoint;
	}

	#endregion
}

}
