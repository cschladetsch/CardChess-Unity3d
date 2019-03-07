using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using CoLib.Utils;

namespace CoLib
{

public static class CubicBezierTangentCoefficientsUtils
{
	#region Public static methods

	/// <summary>
	/// Gets the velocity at a point of the curve.
	/// </summary>
	/// <returns>The tangent at the given time.</returns>
	/// <param name="coeffs">The precalculated coefficients of the curve.</param>
	/// <param name="t">
	/// The time along the curve, where t=0 is the start point, and t=1 is the end point. Note that t can be extrapolated
	/// beyond these bounds.
	/// </param>
	public static Vector3 GetVelocity(this CubicBezierTangentCoefficients coeffs, double t)
	{
		return (float)(t * t) * coeffs.Coeff1 + ((float)t) * coeffs.Coeff2 + coeffs.Coeff3;
	}

	/// <summary>
	/// Gets the tangent at a point along the curve.
	/// </summary>
	/// <returns>The tangent at the given time.</returns>
	/// <param name="coeffs">The precalculated coefficients of the curve.</param>
	/// <param name="t">
	/// The time along the curve, where t=0 is the start point, and t=1 is the end point. Note that t can be extrapolated
	/// beyond these bounds.
	/// </param>
	public static Vector3 GetTangent(this CubicBezierTangentCoefficients coeffs,  double t)
	{
		Vector3 velocity = (float)(t * t) * coeffs.Coeff1 + ((float)t) * coeffs.Coeff2 + coeffs.Coeff3;
		// If the velocity is 0, we look at the acceleration of the path to determine which direction the path is about
		// to travel to.
		if (velocity.magnitude < float.Epsilon) {
			Vector3 acceleration = coeffs.GetAcceleration(t);
			// If acceleration is 0, the entire path is probably a single point. 
			if (acceleration.magnitude < float.Epsilon) {
				if (coeffs.Coeff1.magnitude < float.Epsilon) {
					return Vector3.forward;
				}
				return coeffs.Coeff1.normalized;
			}
			return acceleration.normalized;
		}
		return velocity.normalized;
	}

	/// <summary>
	/// Get the acceleration at a point along the curve.
	/// </summary>
	/// <returns>The acceleration at a given time.</returns>
	/// <param name="coeffs">The precalculated coefficients of the curve.</param>
	/// <param name="t">
	/// The time along the curve, where t=0 is the start point, and t=1 is the end point. Note that t can be extrapolated
	/// beyond these bounds.
	/// </param>	
	public static Vector3 GetAcceleration(this CubicBezierTangentCoefficients coeffs, double t)
	{
		return ((float)t) * coeffs.Coeff1 * 2 + coeffs.Coeff2;
	}

	/// <summary>
	/// Finds the points of inflection of the curve.
	/// </summary>
	/// <returns>The points of inflection times.</returns>
	/// <param name="segment">Segment.</param>
	public static double[] GetPointsOfInflectionTimes(this CubicBezierTangentCoefficients coeffs)
	{
		// Solve the x,y and z axis separately.
		IEnumerable<double> xRoots = GetCubicExtremities(
			coeffs.Coeff1.x, coeffs.Coeff2.x, coeffs.Coeff3.x);
		IEnumerable<double> yRoots = GetCubicExtremities(
			coeffs.Coeff1.y, coeffs.Coeff2.y, coeffs.Coeff3.y);
		IEnumerable<double> zRoots = GetCubicExtremities(
			coeffs.Coeff1.z, coeffs.Coeff2.z, coeffs.Coeff3.z);
		
		// Join them together, and resort.
		return xRoots
			.Union(yRoots)
			.Union(zRoots)
			.Where(t => 0.0 <= t && t <= 1.0) // In Range of bezier.
			.OrderBy(root => root)
			.ToArray();
	}

	#endregion

	#region Private methods

	private static IEnumerable<double> GetCubicExtremities(float a, float b, float c)
	{
		if (a == 0f)
		{
			if (b != 0f)
			{
				// The case where the bezier is actually just a regular quadratic.
				return new double[] { -c / b };
			}
			// The bezier is a straight line, or a singular point.
			return new double[] {};
		}

		// These are the point of inflection times. (Velocity = 0)
		double[] roots = SolveQuadraticRoots(a, b, c);

		// This is the time with no acceleration.
		double s = -b / (2.0 * a);
		return  roots.Union(new double[]{ s });
	}

	private static double[] SolveQuadraticRoots(double a, double b, double c)
	{
		// Solve the quadratice formula
		// x = (-b +- Sqrt(4ac - b^2)) / (2a);

		double det = b * b - 4 * a * c;

		// No solutions case.
		if (det < 0.0) { return new double[] {}; }

		// 1 solution case.
		if (det == 0)
		{
			return new double[] { -b / (2.0 * a) };
		}

		// 2 solutions case.
		double sqrtDet = Math.Sqrt(det);
		return new double[] { (-b + sqrtDet) / (2.0 * a), (-b - sqrtDet) / (2.0 * a) };
	}


	#endregion
}

}