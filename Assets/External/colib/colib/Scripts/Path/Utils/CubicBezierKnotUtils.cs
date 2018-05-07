using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace CoLib
{

public static class CubicBezierKnotUtils
{
	#region Public static methods

	/// <summary>
	/// Creates a list of CubicBezierKnots automatically generating control points so the path is C1 and C2 continous, 
	/// (meaning the tangent and acceleration at the knots in the path are the same). This method uses the 
	/// Kochanek-Bartels algorithm.
	/// </summary>
	/// <returns>The a list of bezier knots.</returns>
	/// <param name="looped">If set to <c>true</c> the curve will be looped.</param>
	/// <param name="points">The positions os the knots, in order.</param>
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
	/// <param name="relativeControlPoints">
	/// Should control points be calculated as offsets from the knot position, or as absolute values in the path's 
	/// coordinate system.
	/// </param>
		public static List<CubicBezierKnot> GeneratePathKnots(bool looped, IEnumerable<Vector3> points, 
		float tension = 0f, float bias  = 0f, float continuity = 0f, bool relativeControlPoints = false)
	{
		Vector3[] pointsArray = points.ToArray();

		List<CubicBezierKnot> knots = new List<CubicBezierKnot>();

		// Special case, for if the path is one point or less.
		if (pointsArray.Length < 2) { 
			foreach (var point in points) {
				var knot = new CubicBezierKnot();
				knot.InControlPoint = point;
				knot.OutControlPoint = point;
				knot.Position = point;
				knot.Type = CubicBezierKnotType.Singular;
			}
			return knots; 
		}		

		for (int i = 0; i < pointsArray.Length; ++i) {
			int prevIndex = i - 1;
			// Wrap the prevIndex around if the path is looped. Otherwise clamp it to zero.
			if (prevIndex < 0) { 
				prevIndex = looped ? prevIndex + pointsArray.Length : 0; 
			}
			// Wrap the nextIndex around if the path is looped. Otherwise clamp it.
			int nextIndex = looped ? (i + 1) % pointsArray.Length : Mathf.Min(i + 1, pointsArray.Length - 1);
			Vector3 prevPoint = pointsArray[prevIndex];
			Vector3 currentPoint = pointsArray[i % pointsArray.Length];
			Vector3 nextPoint = pointsArray[nextIndex];

			// Explanation of how we generate the midpoints can be found here: 
			// https://en.wikipedia.org/wiki/Kochanek%E2%80%93Bartels_spline

			var inControlPoint = -(
				0.5f * (1f - tension) * (1f + bias) * (1f - continuity) * (currentPoint - prevPoint) +
				0.5f * (1f - tension) * (1f - bias) * (1f + continuity) * (nextPoint - currentPoint));
			var outControlPoint = 
				0.5f * (1f - tension) * (1f + bias) * (1f + continuity) * (currentPoint - prevPoint) +
				0.5f * (1f - tension) * (1f - bias) * (1f - continuity) * (nextPoint - currentPoint);

			if (!relativeControlPoints) {
				inControlPoint += currentPoint;
				outControlPoint += currentPoint;
			}

			var knot = new CubicBezierKnot();
			knot.Position = currentPoint;
			knot.InControlPoint = inControlPoint;
			knot.OutControlPoint = outControlPoint;
			knot.Type = CubicBezierKnotType.Continuous;
			knots.Add(knot);
		}
		return knots;
	} 

	/// <summary>
	/// Takes an existing knot, and forces it's control points to be recalculated based on the CubicBezierKnotType. 
	/// </summary>
	/// <param name="knot">The knot to recalculate.</param>
	/// <param name="inControlFixed">
	/// If set to <c>true</c>, the in control point will be considered fixed and the out control point will be 
	/// recalculated. Otherwise the out control point will be considered fixed and the int control point will be 
	/// recalculated.
	/// </param>
	/// <param name="relativeControlPoints">
	/// Should control points be calculated as offsets from the knot position, or as absolute values in the path's 
	/// coordinate system.
	/// </param>	
	public static void RecalculateControlPoint(this CubicBezierKnot knot, bool inControlFixed = true, 
		bool relativeControlPoints = false)
	{
		Vector3 fixedControlPoint = inControlFixed ? knot.InControlPoint : knot.OutControlPoint;
		Vector3 freeControlPoint = inControlFixed ? knot.OutControlPoint : knot.InControlPoint;
		if (!relativeControlPoints) {
			fixedControlPoint -= knot.Position;
			freeControlPoint -= knot.Position;
		}

		switch (knot.Type) {
			case CubicBezierKnotType.Continuous:
				freeControlPoint = -fixedControlPoint;
				break;
			case CubicBezierKnotType.Aligned:
				Vector3 tangent = -fixedControlPoint.normalized;
				float magnitude = freeControlPoint.magnitude;
				freeControlPoint = tangent * magnitude;
				break;
			case CubicBezierKnotType.Singular:
				fixedControlPoint = Vector3.zero;
				freeControlPoint = Vector3.zero;
				break;
		}

		if (!relativeControlPoints) {
			fixedControlPoint += knot.Position;
			freeControlPoint += knot.Position;
		}

		knot.InControlPoint = inControlFixed ? fixedControlPoint : freeControlPoint;
		knot.OutControlPoint = inControlFixed ? freeControlPoint : fixedControlPoint;
	}

	#endregion
}

}
