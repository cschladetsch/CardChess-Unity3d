using System.Collections.Generic;

using UnityEngine;

namespace CoLib
{

/// <summary>
/// A representation of a path.
/// </summary>
public interface IPath 
{
	/// <summary>
	/// The length of the path.
	/// </summary>
	float Length { get; }

	/// <summary>
	/// Gets a point along the path.
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="t">
	///	The time along the path, where 0 is the start, and 1 is the end of the path.
	/// </param>
	Vector3 GetPoint(double t);

	/// <summary>
	/// Gets the tangent at a certain time along the path.
	/// </summary>
	/// <returns>The tangent at the given time.</returns>
	/// <param name="t">
	///	The time along the path, where 0 is the start, and 1 is the end of the path.
	/// </param>
	Vector3 GetTangent(double t);

	/// <summary>
	/// Get a vector perpendicular to the path at a certain time. Note that 3d lines
	/// have an infinite number of possible normals. This method is should provide a sensible
	/// default.
	/// </summary>
	/// <returns>A normal at a given time.</returns>
	/// <param name="t">	
	///	The time along the path, where 0 is the start, and 1 is the end of the path.
	///</param>
	Vector3 GetNormal(double t);

	/// <summary>
	/// Get a vector perpendicular to the path at a certain time. Note that 3d lines
	/// have an infinite number of possible normals. This method will try to find the vector closest to a given 
	/// direction.
	/// </summary>
	/// <returns>The normal.</returns>
	/// <returns>A normal at a given time.</returns>
	/// <param name="t">	
	///	The time along the path, where 0 is the start, and 1 is the end of the path.
	/// </param>
	/// <param name="up">
	/// Which direction should be considered up.
	/// </param>
	Vector3 GetNormal(double t, Vector3 up);

	/// <summary>
	/// Gets the acceleration vector of the path at a certain time. Acceleration is a property of
	/// various parametric curves, given by their second order derivitive.
	/// </summary>
	/// <returns>The acceleration at a given time. This can be Vector3.zero.</returns>
	/// <param name="t">T.</param>
	Vector3 GetAcceleration(double t);

	/// <summary>
	/// Gets the rotation at a certain time along the path.
	/// </summary>
	/// <returns>The rotation at the given time.</returns>
	/// <param name="t">
	///	The time along the path, where 0 is the start, and 1 is the end of the path.
	/// </param>
	Quaternion GetRotation(double t);

	/// <summary>
	/// Gets the rotation at a certain time along the path.
	/// </summary>
	/// <returns>The rotation at the given time.</returns>
	/// <param name="t">
	///	The time along the path, where 0 is the start, and 1 is the end of the path.
	/// </param>
	/// <param name="t">
	/// The up direction which should be used.
	/// </param>
	Quaternion GetRotation(double t, Vector3 up);

	/// <summary>
	/// Some paths may remap t values to do operations like length normalization. This method provides the inverse
	/// of those operations.
	/// </summary>
	/// <returns>The unmapped time.</returns>
	/// <param name="t">A time value.</param>
	double GetUnmappedTime(double t);

	/// <summary>
	/// Some paths may remap t values to do operations like length normalization. This method performs that operation.
	/// </summary>
	/// <returns>The mapped time.</returns>
	/// <param name="t">A time value.</param>
	double GetMappedTime(double t);

	/// <summary>
	/// Gets the point of the line with major changes in the direction of velocity or acceleration, such as points
	/// of inflection, or join points.
	/// </summary>
	/// <returns>The points of inflection.</returns>
	IEnumerable<double> GetCriticalPoints();
}

}
