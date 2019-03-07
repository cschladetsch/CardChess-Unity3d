using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using CoLib.Utils;

namespace CoLib
{

/// <summary>
/// A path which connects two points.
/// </summary>
public class BezierPath: IPath
{
	#region Public methods

	/// <summary>
	/// Creates a quadratic bezier path connecting two points.
	/// </summary>
	/// <param name="startPoint">The start of the path.</param>
	/// <param name="controlPoint">The control point.</param>
	/// <param name="endPoint">The end of the path.</param>
	public BezierPath(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint)
	{
		_segment = new CubicBezierSegment(startPoint, controlPoint, endPoint);
		_length = -1f;
	}

	/// <summary>
	/// Creates a cubic bezier path connecting two points.
	/// </summary>
	/// <param name="startPoint">The start of the path.</param>
	/// <param name="startControlPoint">The first control point.</param>
	/// <param name="startControlPoint">The second control point.</param>
	/// <param name="endPoint">The end of the path.</param>
	public BezierPath(Vector3 startPoint, Vector3 startControlPoint, Vector3 endControlPoint, Vector3 endPoint)
	{
		_segment = new CubicBezierSegment(startPoint, startControlPoint, endControlPoint, endPoint);
		_length = -1f;
	}

	#endregion

	#region IPath

	/// <summary>
	/// Lazily calculates an approximate length of the path. Calculating the length of a bezier can be an expensive 
	/// operation.
	/// </summary>
	/// <value>The length.</value>
	public float Length 
	{ 
		get 
		{	
			if (_length < 0f) {
				_length = _segment.CalculateLength();
			}
			return _length; 
		} 
	}

	public Vector3 GetPoint(double t)
	{
		return _segment.GetPoint(t);
	}

	public Vector3 GetTangent(double t)
	{
		return Coefficients.GetTangent(t);
	}

	public Vector3 GetNormal(double t)
	{
		return _segment.GetNormal(_coefficients, t);
	}

	public Vector3 GetNormal(double t, Vector3 up)
	{
		return _segment.GetNormal(_coefficients, t, up);
	}

	public Vector3 GetAcceleration(double t)
	{
		return Coefficients.GetAcceleration(t);
	}

	public Quaternion GetRotation(double t)
	{	
		return Quaternion.LookRotation(GetTangent(t), GetNormal(t));
	}

	public Quaternion GetRotation(double t, Vector3 up)
	{
		return Quaternion.LookRotation(GetTangent(t), GetNormal(t, up));
	}

	public IEnumerable<double> GetCriticalPoints() 
	{ 
		var times = Coefficients.GetPointsOfInflectionTimes()
			.ToList();
		times.Insert(0, 0.0);
		times.Insert(times.Count, 1.0);
		return times.Distinct(); 
	}

	public double GetUnmappedTime(double t)
	{
		return t;
	}


	public double GetMappedTime(double t)
	{
		return t;
	}

	#endregion

	#region Private fields

	private CubicBezierTangentCoefficients Coefficients
	{
		get 
		{
			if (_coefficients == null)
			{
				_coefficients = _segment.CalculateTangentCoefficients();
			}
			return _coefficients;
		}
	}

	private readonly CubicBezierSegment _segment;
	private CubicBezierTangentCoefficients _coefficients;
	private float _length;

	#endregion
}

}
