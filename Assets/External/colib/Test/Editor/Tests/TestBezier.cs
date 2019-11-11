/*using System;
using UnityEngine;

using CoLib;
using NUnit.Framework;

namespace CoLib
{

//[TestFixture]
// BROKEN
[Category("Bezier Path Operations")]
internal class TestBezier
{
	public static void AreEqual(Vector3 expected, Vector3 actual, float tolerance, string message = null)
	{
		message = message ?? string.Format("Expected {0} but actual was {1}", 
			expected.ToString("0.0000"), 
			actual.ToString("0.0000")
		);
		Assert.IsTrue((expected - actual).magnitude < tolerance, message);
	}

	public static void AssertVelocityConstant(IPath path, float errorTolerance = 0.05f)
	{
		const int Iterations = 120;

		Vector3 lastPosition = path.GetPoint(0.0);

		float targetDiff = path.Length / Iterations;
		for (int i = 1; i < Iterations; i++)
		{
			double t = i / (double) Iterations;
			Vector3 newPosition = path.GetPoint(t);
			float diff = Vector3.Distance(newPosition, lastPosition);
			float error = Mathf.Abs( (diff - targetDiff) / targetDiff);
			Assert.IsTrue(error < errorTolerance, string.Format("Expected diff of {0} but got {1}", targetDiff, diff)); 
			lastPosition = newPosition;
		}
	}

	public static void AssertMappingReasonable(IPath path)
	{
		for (int i = 0; i <= 60; ++i) 
		{
			double t = i / 60.0;
			double newT = path.GetUnmappedTime(path.GetMappedTime(t));
			Assert.AreEqual(t, newT, 0.001);
		}
	}

	public static void AssertTangentSensible(IPath path, float angleErrorTolerance = 1f)
	{
		// Using a higher iteration count gives better precision of the tangent.
		const int Iterations = 8192;

		Vector3 lastPosition = path.GetPoint(0.0);

		// For comparing the tangent we approximate the tangent by sampling the curve, and looking at the direction to 
		// the next sample.
		for (int i = 1; i < Iterations; i++)
		{
			double t = i / (double) Iterations;
			double lastT = (i - 1) / (double) Iterations;

			Vector3 newPosition = path.GetPoint(t);

			Vector3 approximateTangent = (newPosition - lastPosition).normalized;
			Vector3 tangent = path.GetTangent(lastT);

			float error = Vector3.Angle(approximateTangent, tangent); 

			Assert.IsTrue(error < angleErrorTolerance, string.Format("Expected diff of {0} but got {1}", 
				approximateTangent.ToString("0.0000"), 
				tangent.ToString("0.0000"))
			); 
			lastPosition = newPosition;
		}
	}

	#region Cubic BezierPath tests

	[Test]	
	public static void TestCubicBezierLength1()
	{
		const float EXPECTED_LENGTH = 320.894697f;
		Assert.AreEqual(EXPECTED_LENGTH, TestPath1.Length, 0.1f);
	}

	[Test]	
	public static void TestCubicBezierLength2()
	{
		const float EXPECTED_LENGTH = 206.34359f;
		Assert.AreEqual(EXPECTED_LENGTH, TestPath2.Length, 0.1f);
	}

	[Test]	
	public static void TestCubicBezierLength3()
	{
		const float EXPECTED_LENGTH = 329.935499f;
		Assert.AreEqual(EXPECTED_LENGTH, TestPath3.Length, 0.1f);
	}

	[Test]	
	public static void TestCubicBezierLength4()
	{
		const float EXPECTED_LENGTH = 355.52668f;
		Assert.AreEqual(EXPECTED_LENGTH, TestPath4.Length, 0.4f);
	}

	[Test, MaxTime(10)]	
	public static void TestCubicBezierLengthSpeed()
	{
		float totalPath = 0f;
		// 400 length calculations
		for (int i = 0; i < 100; ++i)
		{
			totalPath += TestPath1.Length + TestPath2.Length + TestPath3.Length + TestPath4.Length;
		}
		Assert.IsTrue(totalPath > 0f);
	}

	[Test]
	public static void TestCubicBezierPosition1()
	{
		AreEqual(new Vector3(73.7f, 152.77f, 0f), TestPath1.GetPoint(0.2), 0.01f);
	}

	[Test]
	public static void TestCubicBezierPosition2()
	{
		AreEqual(new Vector3(190.68f, 0f, 117.51f), TestPath2.GetPoint(0.8), 0.01f);
	}

	[Test]
	public static void TestCubicBezierPosition3()
	{
		AreEqual(new Vector3(0f, 131.88f, 201f), TestPath3.GetPoint(0.5), 0.01f);
	}

	[Test]
	public static void TestCubicBezierPosition4()
	{
		AreEqual(new Vector3(0f, 168f, 44f), TestPath4.GetPoint(0.0), 0.01f);
	}

	[Test]
	public static void TestCubicBezierPosition5()
	{
		AreEqual(new Vector3(0f, 227f, 50f), TestPath4.GetPoint(1.0), 0.01f);
	}

	[Test]
	public static void TestCubicBezierTangent1()
	{
		Vector3 tangent = new Vector3(-38f, 101f, 0f).normalized;
		AreEqual(tangent, TestPath1.GetTangent(0.0), 0.01f);
	}

	[Test]
	public static void TestCubicBezierTangent2()
	{
		var path = TestPath1;
		AssertTangentSensible(path);
	}

	[Test]
	public static void TestCubicBezierTangent3()
	{
		var path = TestPath2;
		AssertTangentSensible(path);
	}

	[Test]
	public static void TestCubicBezierTangent4()
	{
		var path = TestPath3;
		AssertTangentSensible(path);
	}

	[Test]
	public static void TestCubicBezierTangent5()
	{
		var path = TestPath4;
		AssertTangentSensible(path);
	}

	#endregion

	#region Quadratic BezierPath tests

	[Test]
	public static void TestQuadraticBezierPathPosition()
	{
		AreEqual(new Vector3(0f, 57.08f, 129.68f), TestQuadraticPath1.GetPoint(0.2), 0.01f);
	}

	#endregion

	#region Cubic NormalizedBezierPath tests

	[Test]	
	public static void TestNormalizedCubicBezierLength1()
	{
		const float EXPRECTED_LENGTH = 320.894697f;
		Assert.AreEqual(EXPRECTED_LENGTH, TestNormalizedPath1.Length, 0.1f);
	}

	[Test]	
	public static void TestNormalizedCubicBezierLength2()
	{
		const float EXPRECTED_LENGTH = 206.34359f;
		Assert.AreEqual(EXPRECTED_LENGTH, TestNormalizedPath2.Length, 0.1f);
	}

	[Test]	
	public static void TestNormalizedCubicBezierLength3()
	{
		const float EXPRECTED_LENGTH = 329.935499f;
		Assert.AreEqual(EXPRECTED_LENGTH, TestNormalizedPath3.Length, 0.1f);
	}

	[Test]	
	public static void TestNormalizedCubicBezierLength4()
	{
		const float EXPRECTED_LENGTH = 355.52668f;
		Assert.AreEqual(EXPRECTED_LENGTH, TestNormalizedPath4.Length, 0.1f);
	}

	[Test]
	public static void TestNormalizedCubicBezierLength5()
	{
		Assert.AreEqual(TestPath5.Length, TestNormalizedPath5.Length, 0.6f);
	}

	[Test, MaxTime(15)]	
	public static void TestNormalizedCubicBezierLengthSpeed()
	{
		float totalPath = 0f;
		// 400 length calculations
		for (int i = 0; i < 100; ++i)
		{
			totalPath += TestPath1.Length + TestPath2.Length + TestPath3.Length + TestPath4.Length;
		}
		Assert.IsTrue(totalPath > 0f);
	}

	[Test]
	public static void TestNormalizedCubicBezierVelocity1()
	{
		AssertVelocityConstant(TestNormalizedPath1);
	}

	[Test]
	public static void TestNormalizedCubicBezierVelocity2()
	{
		AssertVelocityConstant(TestNormalizedPath2);
	}

	[Test]
	public static void TestNormalizedCubicBezierVelocity3()
	{
		AssertVelocityConstant(TestNormalizedPath3);
	}

	[Test]
	public static void TestNormalizedCubicBezierVelocity4()
	{
		AssertVelocityConstant(TestNormalizedPath4);
	}

	[Test]
	public static void TestNormalizedCubicBezierTimeMapping1()
	{
		AssertMappingReasonable(TestNormalizedPath1);
	}

	[Test]
	public static void TestNormalizedCubicBezierTimeMapping2()
	{
		AssertMappingReasonable(TestNormalizedPath2);
	}

	[Test]
	public static void TestNormalizedCubicBezierTimeMapping3()
	{
		AssertMappingReasonable(TestNormalizedPath3);
	}

	[Test]
	public static void TestNormalizedCubicBezierTimeMapping4()
	{
		AssertMappingReasonable(TestNormalizedPath4);
	}

	//[Test]
    // BORKEN
	public static void TestAutoRotationEquivalence()
	{
		var p1 = new AutoWindingPath(TestPath1, Vector3.up);
		var p2 = new AutoWindingPath(TestNormalizedPath1, Vector3.up);

		for (int i = 0; i <= 200; ++i)
		{
			double t = i / 200.0;
			double adjustedT = p2.GetUnmappedTime(t);
			Vector3 norm1 = p1.GetNormal(t);
			Vector3 norm2 = p2.GetNormal(adjustedT);
			AreEqual(norm1, norm2, 0.01f);
		}
	}

	#endregion

	#region Test Data

	private static IPath TestPath1 
	{
		get 
		{ 
			return new BezierPath(
				new Vector3(73f, 99f, 0f),
				new Vector3(35f, 200f, 0f),
				new Vector3(220f, 260f, 0f),
				new Vector3(220f, 40f, 0f)
			);
		}
	}

	private static IPath TestPath2  
	{
		get 
		{ 
			return new BezierPath(
				new Vector3(23f, 0f, 101f),
				new Vector3(127f, 0f, 133f),
				new Vector3(167f, 0f, 132f),
				new Vector3(223f, 0f, 104f)
			);
		}
	}

	private static IPath TestPath3  
	{
		get 
		{ 
			return new BezierPath(
				new Vector3(0f, 147f, 50f),
				new Vector3(0f, 27f, 254f),
				new Vector3(0f, 226f, 245f),
				new Vector3(0f, 149f, 61f)
			);
		}
	}

	private static IPath TestPath4
	{
		get 
		{ 
			return new BezierPath(
				new Vector3(0f, 168f, 44f),
				new Vector3(0f, 220f, 253f),
				new Vector3(0f, 25f, 249f),
				new Vector3(0f, 227f, 50f)
			);
		}
	}

	private static IPath TestPath5
	{
		get 
		{
			return new BezierPath(
				new Vector3(-20.03161f, 12.49755f, -22.88328f),
				new Vector3(-20.03161f -34.40668f, 12.49755f-78.27212f, 170.8581f-22.88328f),
				new Vector3(-16.55028f, -0.5663757f, -25.35486f),
				new Vector3(-16.55028f, -0.5663757f, -25.35486f)
			);
		}	
	}

	private static IPath TestQuadraticPath1
	{
		get
		{
			// Should translate to a cubic with control points (0, 53, 82), (0, 113, 72)
			return new BezierPath(
				new Vector3(0f, 47f, 176f),
				new Vector3(0f, 56f, 35f), 
				new Vector3(0f, 227f, 146f)
			);
		}
	}

	private static IPath TestNormalizedPath1 
	{
		get 
		{ 
			return new NormalizedBezierPath(
				new Vector3(73f, 99f, 0f),
				new Vector3(35f, 200f, 0f),
				new Vector3(220f, 260f, 0f),
				new Vector3(220f, 40f, 0f)
			);
		}
	}

	private static IPath TestNormalizedPath2  
	{
		get 
		{ 
			return new NormalizedBezierPath(
				new Vector3(23f, 0f, 101f),
				new Vector3(127f, 0f, 133f),
				new Vector3(167f, 0f, 132f),
				new Vector3(223f, 0f, 104f)
			);
		}
	}

	private static IPath TestNormalizedPath3  
	{
		get 
		{ 
			return new NormalizedBezierPath(
				new Vector3(0f, 147f, 50f),
				new Vector3(0f, 27f, 254f),
				new Vector3(0f, 226f, 245f),
				new Vector3(0f, 149f, 61f)
			);
		}
	}

	private static IPath TestNormalizedPath4
	{
		get 
		{ 
			return new NormalizedBezierPath(
				new Vector3(0f, 168f, 44f),
				new Vector3(0f, 220f, 253f),
				new Vector3(0f, 25f, 249f),
				new Vector3(0f, 227f, 50f)
			);
		}
	}

	private static IPath TestNormalizedPath5
	{
		get 
		{
			return new NormalizedBezierPath(
				new Vector3(-20.03161f, 12.49755f, -22.88328f),
				new Vector3(-20.03161f -34.40668f, 12.49755f-78.27212f, 170.8581f-22.88328f),
				new Vector3(-16.55028f, -0.5663757f, -25.35486f),
				new Vector3(-16.55028f, -0.5663757f, -25.35486f)
			);
		}	
	}

	#endregion
}

}

*/