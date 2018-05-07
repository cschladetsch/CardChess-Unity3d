using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using CoLib.Utils;

namespace CoLib
{

public static class CubicBezierUtils
{
	#region Public static methods

	/// <summary>
	/// Calculates the length of a CubicBezierSegment. Note that because this uses a numerical method to get the length,
	/// it won't be completely accurate.
	/// </summary>
	/// <returns>The length of the bezier segment.</returns>
	/// <param name="segment">The segment to calculate the length of.</param>
	public static float CalculateLength(this CubicBezierSegment segment)
	{
		// Subdividing the segment into 4 pieces, then getting an approximate length on those 4 pieces, gives
		// a fairly accurate length (< 1% error), and is very quick. 
		CubicBezierSegment l = new CubicBezierSegment(), r = new CubicBezierSegment(), 
			ll = new CubicBezierSegment(), lr = new CubicBezierSegment(), rl = new CubicBezierSegment(),
			rr = new CubicBezierSegment();
		segment.Subdivide(l, r, 0.5);
		l.Subdivide(ll, lr, 0.5);
		r.Subdivide(rl, rr, 0.5);
		return ll.CalculateApproximateLength() + lr.CalculateApproximateLength() + rl.CalculateApproximateLength() + 
			rr.CalculateApproximateLength();
	}

	/// <summary>
	/// Calculates the tangent coefficents of a segment. This is used in tangent and acceleration calculations.
	/// </summary>
	/// <returns>The tangent coefficients.</returns>
	/// <param name="segment">The segment to calculate the coefficients of.</param>
	public static CubicBezierTangentCoefficients CalculateTangentCoefficients(this CubicBezierSegment segment)
	{
		var coeffs = new CubicBezierTangentCoefficients();

		coeffs.Coeff1 = -3f * segment.StartPoint + 9f * segment.StartControlPoint - 9 * segment.EndControlPoint + 3f * segment.EndPoint;
		coeffs.Coeff2 = 6f * segment.StartPoint - 12f * segment.StartControlPoint + 6 * segment.EndControlPoint;
		coeffs.Coeff3 = -3f * segment.StartPoint + 3f * segment.StartControlPoint;

		return coeffs;
	}

	/// <summary>
	/// Subdivides a bezier curve segment at a specific point.
	/// </summary>
	/// <param name="segment">The segment to subdivide.</param>
	/// <param name="leftSegment">The segment to write the left segment data into. </param>
	/// <param name="rightSegment">The segment to write the right segment data into.</param>
	/// <param name="t">The time along the curve to subdivide at, with range [0, 1]</param>
	public static void Subdivide(this CubicBezierSegment segment, CubicBezierSegment startSegment, 
		CubicBezierSegment leftSegment, double t)
	{
		if (t < 0 || t > 1) {
			throw new ArgumentOutOfRangeException("t");
		}
		float posT = (float) t;
		Vector3 e = Vector3.Lerp(segment.StartPoint, segment.StartControlPoint, posT);
		Vector3 f = Vector3.Lerp(segment.StartControlPoint, segment.EndControlPoint, posT);
		Vector3 g = Vector3.Lerp(segment.EndControlPoint, segment.EndPoint, posT);
		Vector3 h = Vector3.Lerp(e, f, posT);
		Vector3 j = Vector3.Lerp(f, g, posT);
		Vector3 k = Vector3.Lerp(h, j, posT);

		startSegment.StartPoint = segment.StartPoint;
		startSegment.StartControlPoint = e;
		startSegment.EndControlPoint = h;
		startSegment.EndPoint = k;

		leftSegment.StartPoint = k;
		leftSegment.StartControlPoint = j;
		leftSegment.EndControlPoint = g;
		leftSegment.EndPoint = segment.EndPoint;
	}

	/// <summary>
	/// Gets a point along the curve.
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="segment">The segment to calculate the point of.</param>
	/// <param name="t">
	/// The time along the curve, where t=0 is the start point, and t=1 is the end point. Note that t can be extrapolated
	/// beyond these bounds.
	/// </param>
	public static Vector3 GetPoint(this CubicBezierSegment segment, double t)
	{
		double t2 = t * t;
  		double t3 = t2 * t;
  		double mt = 1.0 - t;
  		double mt2 = mt * mt;
  		double mt3 = mt2 * mt;

  		return 
  			(float) mt3 * segment.StartPoint + 
  			(float) (3 * mt2 * t) * segment.StartControlPoint + 
  			(float) (3 * mt * t2) * segment.EndControlPoint + 
  			(float) t3 * segment.EndPoint;
	}

	/// <summary>
	/// Gets a normal for a given segment. Note that there are an infinite number of possible normals, this method just
	/// produces a locally consistent normal.
	/// </summary>
	/// <returns>A normal at a given point.</returns>
	/// <param name="segment">The segment to calculate the tangent of.</param>
	/// <param name="segment">The precalculated coefficients of the curve.</param>
	/// <param name="t">
	/// The time along the curve, where t=0 is the start point, and t=1 is the end point. Note that t can be extrapolated
	/// beyond these bounds.
	/// </param>
	public static Vector3 GetNormal(this CubicBezierSegment segment, CubicBezierTangentCoefficients coeffs, double t)
	{
		Vector3 currentTangent = coeffs.GetTangent(t);

		Vector3 normal;

		if (currentTangent == Vector3.zero) {
			Vector3 acceleration = coeffs.GetAcceleration(t);
			currentTangent = acceleration;
		}

		// The tangent doesn't change, (such in the case of a straight line).
		// We pick a vector which shouldn't be parallel to the tangent, and
		// use the cross product with the tangent to find a normal.
		normal = currentTangent;
		normal.x = -currentTangent.y;
		normal.y = -currentTangent.z;
		normal.z = currentTangent.x;

		normal = Vector3.Cross(currentTangent, normal).normalized;
		return normal;
	}

	/// <summary>
	/// Gets a normal for a given segment. Note that there are an infinite number of possible normals, this method just
	/// produces a locally consistent normal.
	/// </summary>
	/// <returns>A normal at a given point.</returns>
	/// <param name="segment">The segment to calculate the tangent of.</param>
	/// <param name="segment">The precalculated coefficients of the curve.</param>
	/// <param name="t">
	/// The time along the curve, where t=0 is the start point, and t=1 is the end point. Note that t can be extrapolated
	/// beyond these bounds.
	/// </param>
	/// <param name="up">
	/// The direction the normal should try to match.
	/// </param>
	public static Vector3 GetNormal(this CubicBezierSegment segment, CubicBezierTangentCoefficients coeffs, double t, Vector3 up)
	{
		Vector3 baseNormal = segment.GetNormal(coeffs, t);
		Vector3 tangent = coeffs.GetTangent(t);

		Vector3 upNormal = Vector3.Cross(Vector3.Cross(tangent, up), tangent).normalized;

		if (Vector3.Angle(tangent, up) == 0f) { return baseNormal; }

		float angle = Mathf.Min( Mathf.Abs(Vector3.Angle(tangent, up)), Mathf.Abs(Vector3.Angle(tangent, -up)));
		float lerp = Mathf.Clamp01( (angle + 5) / 45f);
		return AngleLerp(baseNormal, upNormal, lerp);
	}

	public static Vector3 AngleLerp(this Vector3 first, Vector3 second, float t)
	{
		Vector3 tangent = Vector3.Cross(first, second).normalized;
		if (first == second) { return first; }
		Quaternion firstLook =  Quaternion.LookRotation(first,tangent);
		Quaternion secondLook = Quaternion.LookRotation(second, tangent);

		Quaternion interpolated = Quaternion.Slerp(firstLook, secondLook, t);

		return interpolated * Vector3.forward;
	}

	#endregion

	#region Private static methods

	private static Vector3 GetQuadraticApproximationControlPoint(this CubicBezierSegment segment)
	{
		return
			(3 * segment.EndControlPoint - segment.EndPoint + 3 * segment.StartControlPoint - segment.StartPoint) * 0.25f; 
	}

	private static float CalculateApproximateLength(this CubicBezierSegment segment)
	{
		// Approximate the curve using a quadratic bezier.
		Vector3 controlPoint = segment.GetQuadraticApproximationControlPoint();
		Vector3 v1 = controlPoint - segment.StartPoint;
		Vector3 v2 = segment.StartPoint - 2 * controlPoint + segment.EndPoint;

		// Calculate the exact length of the quadratic bezier by evaluating it's integral.
		// http://stackoverflow.com/a/11857788/814164
		if (v2 != Vector3.zero) {
			double c = 4 * Vector3.Dot(v2,v2);
			double b = 8 * Vector3.Dot(v1,v2);
			double a = 4 * Vector3.Dot(v1,v1);
			double q = 4 * a * c - b * b;

			double twoCpB = 2 * c + b;
			double sumCBA = c + b + a;
			double mult0 = 0.25 / c;
			double mult1 = q / (8 * Math.Pow(c, 1.5));
			double length =
				mult0 * (twoCpB * Math.Sqrt(sumCBA) - b * Math.Sqrt(a)) +
				mult1 * (Math.Log(2 * Math.Sqrt(c * sumCBA) + twoCpB) - Math.Log(2 * Math.Sqrt(c * a) + b));

			return double.IsNaN(length) ? v1.magnitude + (segment.EndPoint - controlPoint).magnitude : (float) length;
		}
		else { return 2 * v1.magnitude; };
	}

	#endregion
}

}

