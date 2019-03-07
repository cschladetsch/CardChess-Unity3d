using System.Collections.Generic;

using UnityEngine;

using CoLib.Utils;

namespace CoLib
{

/// <summary>
/// A path which linearly connects two points.
/// </summary>
public class LinearPath: IPath
{
	#region Public methods

	/// <summary>
	/// Creates a linear path connecting two points.
	/// </summary>
	/// <param name="startPoint">The start of the path.</param>
	/// <param name="endPoint">The end of the path.</param>
	public LinearPath(Vector3 startPoint, Vector3 endPoint)
	{
		_startPoint = startPoint;
		_endPoint = endPoint;
		_length = (_endPoint - _startPoint).magnitude;
	}

	#endregion

	#region IPath

	public float Length { get { return _length; } }

	public Vector3 GetPoint(double t)
	{
		return Vector3.LerpUnclamped(_startPoint, _endPoint, (float)t);
	}

	public Vector3 GetTangent(double t)
	{
		if (_endPoint == _startPoint) { return Vector3.forward; }
		return (_endPoint - _startPoint).normalized;
	}

	public Vector3 GetNormal(double t) 
	{
		if (_endPoint == _startPoint) { return Vector3.up; }

		Vector3 tangent = GetTangent(t);

		Vector3 normal = tangent;
		normal.x = -tangent.y;
		normal.y = -tangent.z;
		normal.z = tangent.x;
		return Vector3.Cross(tangent, normal).normalized;
	}

	public Vector3 GetNormal(double t, Vector3 up) 
	{
		if (_endPoint == _startPoint) { return up; }

		Vector3 regularNormal = GetNormal(t);
		Vector3 tangent = GetTangent(t);
		if (Vector3.Angle(up, tangent) == 0f) {
			return Vector3.Cross(Vector3.Cross(tangent, up), tangent);
		}

		return regularNormal;
	}

	public Vector3 GetAcceleration(double t)
	{
		return Vector3.zero;
	}

	public Quaternion GetRotation(double t)
	{	
		if (_endPoint == _startPoint) { return Quaternion.identity; }
		return Quaternion.LookRotation(GetTangent(t), GetNormal(t));
	}

	public Quaternion GetRotation(double t, Vector3 up)
	{
		if (_endPoint == _startPoint) { return Quaternion.identity; }

		return Quaternion.LookRotation(GetTangent(t), GetNormal(t, up));
	}

	public IEnumerable<double> GetCriticalPoints() 
	{
		return new double[] { 0.0, 1.0 };
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

	private readonly Vector3 _startPoint;
	private readonly Vector3 _endPoint;
	private readonly float _length;

	#endregion
}

}
