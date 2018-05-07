using System;
using UnityEngine;

namespace CoLib
{

[Serializable]
/// <summary>
/// Contains precalculated coefficients used in tangent and acceleration calculations of a bezier curve.
/// </summary>
public class CubicBezierTangentCoefficients
{
	/// <summary>
	/// First coefficient of tangent function, (t^2)
	/// </summary>
	public Vector3 Coeff1;
	/// <summary>
	/// Second coefficient of tangent function, (t)
	/// </summary>
	public Vector3 Coeff2;
	/// <summary>
	/// Third coefficient of tangent function, (c)
	/// </summary>
	public Vector3 Coeff3;
}

}
