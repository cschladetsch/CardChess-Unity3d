using System;
using UnityEngine;
using CoLib;
using NUnit.Framework;

namespace CoLib
{

[TestFixture]
[Category("Commands~Tween")]
internal class TestCommands_Tween
{	
	public static void AreEqual(double first, double second, double tolerance, string message = null)
	{
		Assert.AreEqual(first, second, tolerance, message);
	}

	public static void AreEqual(Vector4 first, Vector4 second, float tolerance, string message = null)
	{
		Assert.IsTrue((first - second).magnitude < tolerance, message);
	}
	
	public static void AreEqual(Rect firstValue, Rect secondValue, float tolerance, string message = null)
	{
		Assert.IsFalse( (Math.Abs(firstValue.x - secondValue.x) > tolerance ||
		    Math.Abs(firstValue.y - secondValue.y) > tolerance ||
		    Math.Abs(firstValue.width - secondValue.width) > tolerance ||
		    Math.Abs(firstValue.height - secondValue.height) > tolerance), message);
	}
	
	public static void AreEqual(Quaternion firstValue, Quaternion secondValue, float tolerance, string message = null)
	{
		Assert.IsTrue(Math.Abs(1.0f - Quaternion.Dot(firstValue, secondValue)) < tolerance, message);
	}

	[Test]
	public static void TestChangeBy()
	{
		const float floatOffset = 4.8f;
		const float floatStart = 1.2f;
		float floatVal = floatStart;
		Ref<float> floatRef = new Ref<float>(
			() => floatVal,
			t => floatVal = t
		);
		
		const double doubleOffset = 3.2;
		const double doubleStart = 9.2;
		double doubleVal = doubleStart;
		Ref<double> doubleRef = new Ref<double>(
			() => doubleVal,
			t => doubleVal = t
		);
		
		Vector2 vec2Offset = new Vector2(9.5f, 2.0f);
		Vector2 vec2Start = new Vector2(4.0f, 5.0f);
		Vector2 vec2Val = vec2Start;
		Ref<Vector2> vec2Ref = new Ref<Vector2>(
			() => vec2Val,
			t => vec2Val = t
		);
		
		Vector3 vec3Offset = new Vector3(4.0f, 19.0f, 2.0f);
		Vector3 vec3Start = new Vector3(92.0f, 0.5f, 34.0f);
		Vector3 vec3Val = vec3Start;
		Ref<Vector3> vec3Ref = new Ref<Vector3>(
			() => vec3Val,
			t => vec3Val = t
		);
		
		Vector4 vec4Offset = new Vector4(92.0f, 0.5f, 14.0f, 7.0f);
		Vector4 vec4Start = new Vector4(0.4f, 10.0f, 3.0f, 82.0f);
		Vector4 vec4Val = vec4Start;
		Ref<Vector4> vec4Ref = new Ref<Vector4>(
			() => vec4Val,
			t => vec4Val = t
		);
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.Parallel(
					Commands.ChangeBy(floatRef, floatOffset, 1.0), 
					Commands.ChangeBy(doubleRef, doubleOffset, 1.0), 
					Commands.ChangeBy(vec2Ref, vec2Offset, 1.0), 
					Commands.ChangeBy(vec3Ref, vec3Offset, 1.0), 
					Commands.ChangeBy(vec4Ref, vec4Offset, 1.0) 
				)
			)
		);
		
		queue.Update(0.3);
		// Check basic lerping works.
		Assert.AreEqual(floatVal, floatOffset * 0.3f + floatStart, 0.01);
		Assert.AreEqual(doubleVal, doubleOffset * 0.3 + doubleStart, 0.01);
		AreEqual(vec2Val, vec2Offset * 0.3f + vec2Start, 0.01f);
		AreEqual(vec3Val, vec3Offset * 0.3f + vec3Start, 0.01f);
		AreEqual(vec4Val, vec4Offset * 0.3f + vec4Start, 0.01f);
		queue.Update(0.7);
		// Completes the offset
		Assert.AreEqual(floatVal, floatOffset + floatStart, 0.01f);
		Assert.AreEqual(doubleVal, doubleOffset  + doubleStart, 0.01);
		AreEqual(vec2Val, vec2Offset + vec2Start, 0.01f);
		AreEqual(vec3Val, vec3Offset + vec3Start, 0.01f);
		AreEqual(vec4Val, vec4Offset + vec4Start, 0.01f);
		queue.Update(0.3);
		// Check that it compounds the result
		Assert.AreEqual(floatVal, floatOffset * 1.3f + floatStart, 0.01f);
		Assert.AreEqual(doubleVal, doubleOffset * 1.3 + doubleStart, 0.01);
		AreEqual(vec2Val, vec2Offset * 1.3f + vec2Start, 0.01f);
		AreEqual(vec3Val, vec3Offset * 1.3f + vec3Start, 0.01f);
		AreEqual(vec4Val, vec4Offset * 1.3f + vec4Start, 0.01f);
		// Reset the vals to zero.
		floatVal = 0.0f;
		doubleVal = 0.0;
		vec2Val = Vector2.zero;
		vec3Val = Vector3.zero;
		vec4Val = Vector4.zero;
		queue.Update(0.7);
		// And check the offset continues.
		Assert.AreEqual(floatVal, floatOffset * 0.7f, 0.01f);
		Assert.AreEqual(doubleVal, doubleOffset * 0.7, 0.01);
		AreEqual(vec2Val, vec2Offset * 0.7f, 0.01f);
		AreEqual(vec3Val, vec3Offset * 0.7f, 0.01f);
		AreEqual(vec4Val, vec4Offset * 0.7f, 0.01f);
	}

	[Test]
	public static void TestChangeTo()
	{
		const float floatEnd = 4.8f;
		const float floatStart = 1.2f;
		float floatVal = floatStart;
		Ref<float> floatRef = new Ref<float>(
			() => floatVal,
			t => floatVal = t
		);
		
		const double doubleEnd = 3.2;
		const double doubleStart = 9.2;
		double doubleVal = doubleStart;
		Ref<double> doubleRef = new Ref<double>(
			() => doubleVal,
			t => doubleVal = t
		);
		
		Vector2 vec2End = new Vector2(9.5f, 2.0f);
		Vector2 vec2Start = new Vector2(4.0f, 5.0f);
		Vector2 vec2Val = vec2Start;
		Ref<Vector2> vec2Ref = new Ref<Vector2>(
			() => vec2Val,
			t => vec2Val = t
		);
		
		Vector3 vec3End = new Vector3(4.0f, 19.0f, 2.0f);
		Vector3 vec3Start = new Vector3(92.0f, 0.5f, 34.0f);
		Vector3 vec3Val = vec3Start;
		Ref<Vector3> vec3Ref = new Ref<Vector3>(
			() => vec3Val,
			t => vec3Val = t
		);
		
		Vector4 vec4End = new Vector4(92.0f, 0.5f, 14.0f, 7.0f);
		Vector4 vec4Start = new Vector4(0.4f, 10.0f, 3.0f, 82.0f);
		Vector4 vec4Val = vec4Start;
		Ref<Vector4> vec4Ref = new Ref<Vector4>(
			() => vec4Val,
			t => vec4Val = t
		);
		
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.Parallel(
					Commands.ChangeTo(floatRef, floatEnd, 1.0), 
					Commands.ChangeTo(doubleRef, doubleEnd, 1.0), 
					Commands.ChangeTo(vec2Ref, vec2End, 1.0), 
					Commands.ChangeTo(vec3Ref, vec3End, 1.0), 
					Commands.ChangeTo(vec4Ref, vec4End, 1.0) 
				)
			)
		);
		
		queue.Update(0.3);
		// Check basic lerping works.
			Assert.AreEqual(floatVal, floatEnd * 0.3f + floatStart * 0.7f, 0.01);
			Assert.AreEqual(doubleVal, doubleEnd * 0.3 + doubleStart * 0.7, 0.01);
		AreEqual(vec2Val, vec2End * 0.3f + vec2Start * 0.7f, 0.01f);
		AreEqual(vec3Val, vec3End * 0.3f + vec3Start * 0.7f, 0.01f);
		AreEqual(vec4Val, vec4End * 0.3f + vec4Start * 0.7f, 0.01f);
		// Reset the vals to zero. Checks that 'ChangeTo' will force itself back on
		// track.
		floatVal = 0.0f;
		doubleVal = 0.0;
		vec2Val = Vector2.zero;
		vec3Val = Vector3.zero;
		vec4Val = Vector4.zero;
		queue.Update(0.2);
		// Completes the offset
			Assert.AreEqual(floatVal, floatEnd * 0.5f + floatStart * 0.5f, 0.01);
			Assert.AreEqual(doubleVal, doubleEnd * 0.5 + doubleStart * 0.5, 0.01);
		AreEqual(vec2Val, vec2End * 0.5f + vec2Start * 0.5f, 0.01f);
		AreEqual(vec3Val, vec3End * 0.5f + vec3Start * 0.5f, 0.01f);
		AreEqual(vec4Val, vec4End * 0.5f + vec4Start * 0.5f, 0.01f);
		queue.Update(0.5);
		
			Assert.AreEqual(floatVal, floatEnd, 0.01f);
			Assert.AreEqual(doubleVal, doubleEnd, 0.01);
		AreEqual(vec2Val, vec2End, 0.01f);
		AreEqual(vec3Val, vec3End, 0.01f);
		AreEqual(vec4Val, vec4End, 0.01f);
		queue.Update(0.5);
		// Check that it doesn't move once it has reached it's final position.
		Assert.AreEqual(floatVal, floatEnd, 0.01f);
		Assert.AreEqual(doubleVal, doubleEnd, 0.01);
		AreEqual(vec2Val, vec2End, 0.01f);
		AreEqual(vec3Val, vec3End, 0.01f);
		AreEqual(vec4Val, vec4End, 0.01f);
		
		Rect rectEnd = new Rect(-1.0f, 1.0f, 5.0f, 5.0f);
		Rect rectStart = new Rect(0.0f,2.0f, 6.0f, 6.0f);
		Rect rectVal = new Rect();
		Ref<Rect> rectRef = new Ref<Rect>(
			() => { return rectVal; },
			t => { rectVal = t; }
		);
		
		Vector2 firstAnchor = new Vector2(0.0f, 0.0f);
		Vector2 secondAnchor = new Vector2(1.0f, 0.0f);
		Vector3 thirdAnchor = new Vector2(0.0f, 1.0f);
		Vector2 forthAnchor = new Vector2(1.0f, 1.0f);
		
		CommandDelegate reset = Commands.Do(() => { rectVal = rectStart; });
		queue = new CommandQueue();
		queue.Enqueue(
			reset,
			Commands.ChangeTo(rectRef, rectEnd, 1.0, firstAnchor),
			Commands.WaitForFrames(1),
			reset,
			Commands.ChangeTo(rectRef, rectEnd, 1.0, secondAnchor),
			Commands.WaitForFrames(1),
			reset,
			Commands.ChangeTo(rectRef, rectEnd, 1.0, thirdAnchor),
			Commands.WaitForFrames(1),
			reset,
			Commands.ChangeTo(rectRef, rectEnd, 1.0, forthAnchor)
		);
		
		// Test the top left corner.
		queue.Update(0.5);
		AreEqual(rectVal, new Rect(
			-0.5f, 1.5f, 
			(rectStart.width + rectEnd.width) * 0.5f,
			(rectStart.height + rectEnd.height) * 0.5f), 0.001f
		);
		queue.Update(0.5);
		AreEqual(rectVal, rectEnd, 0.001f);
		queue.Update(0.0f);
		
		// Test the top right corner.
		queue.Update(0.3);
		AreEqual(rectVal, new Rect(
			5.4f - 5.7f, 1.7f, 
			rectStart.width * 0.7f + rectEnd.width * 0.3f,
			rectStart.height * 0.7f + rectEnd.height * 0.3f), 0.001f
		);
		queue.Update(0.7);
		AreEqual(rectVal, rectEnd, 0.001f);
		queue.Update(0.0f);
		
		// Test the bottom left corner.
		queue.Update(0.4);
		AreEqual(rectVal, new Rect(
			-0.4f, 7.2f - 5.6f, 
			rectStart.width * 0.6f + rectEnd.width * 0.4f,
			rectStart.height * 0.6f + rectEnd.height * 0.4f), 0.001f
		);
		queue.Update(0.6);
		AreEqual(rectVal, rectEnd, 0.001f);
		queue.Update(0.0f);
		
		// Test the bottom right corner.
		queue.Update(0.4);
		AreEqual(rectVal, new Rect(
			5.2f - 5.6f, 7.2f - 5.6f, 
			rectStart.width * 0.6f + rectEnd.width * 0.4f,
			rectStart.height * 0.6f + rectEnd.height * 0.4f), 0.001f
		);
		queue.Update(0.6);
		AreEqual(rectVal, rectEnd, 0.001f);
		queue.Update(0.0f);
		
		
	}

	[Test]
	public static void TestChangeFrom()
	{
		const float floatEnd = 4.8f;
		const float floatStart = 1.2f;
		float floatVal = floatStart;
		Ref<float> floatRef = new Ref<float>(
			() => floatVal,
			t => floatVal = t
		);
		
		const double doubleEnd = 3.2;
		const double doubleStart = 9.2;
		double doubleVal = doubleStart;
		Ref<double> doubleRef = new Ref<double>(
			() => doubleVal,
			t => doubleVal = t
		);
		
		Vector2 vec2End = new Vector2(9.5f, 2.0f);
		Vector2 vec2Start = new Vector2(4.0f, 5.0f);
		Vector2 vec2Val = vec2Start;
		Ref<Vector2> vec2Ref = new Ref<Vector2>(
			() => vec2Val,
			t => vec2Val = t
		);
		
		Vector3 vec3End = new Vector3(4.0f, 19.0f, 2.0f);
		Vector3 vec3Start = new Vector3(92.0f, 0.5f, 34.0f);
		Vector3 vec3Val = vec3Start;
		Ref<Vector3> vec3Ref = new Ref<Vector3>(
			() => vec3Val,
			t => vec3Val = t
		);
		
		Vector4 vec4End = new Vector4(92.0f, 0.5f, 14.0f, 7.0f);
		Vector4 vec4Start = new Vector4(0.4f, 10.0f, 3.0f, 82.0f);
		Vector4 vec4Val = vec4Start;
		Ref<Vector4> vec4Ref = new Ref<Vector4>(
			() => vec4Val,
			t => vec4Val = t
		);
		
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.Sequence(
					Commands.Parallel(
						Commands.ChangeFrom(floatRef, floatEnd, 1.0), 
						Commands.ChangeFrom(doubleRef, doubleEnd, 1.0), 
						Commands.ChangeFrom(vec2Ref, vec2End, 1.0), 
						Commands.ChangeFrom(vec3Ref, vec3End, 1.0), 
						Commands.ChangeFrom(vec4Ref, vec4End, 1.0) 
					),
					Commands.WaitForFrames(1)
				)
			)
		);
		
		queue.Update(0.3);
		// Check basic lerping works.
		AreEqual(floatVal, floatEnd * 0.7f + floatStart * 0.3f, 0.01);
		AreEqual(doubleVal, doubleEnd * 0.7 + doubleStart * 0.3, 0.01);
		AreEqual(vec2Val, vec2End * 0.7f + vec2Start * 0.3f, 0.01f);
		AreEqual(vec3Val, vec3End * 0.7f + vec3Start * 0.3f, 0.01f);
		AreEqual(vec4Val, vec4End * 0.7f + vec4Start * 0.3f, 0.01f);
		// Reset the vals to zero. Checks that 'ChangeTo' will force itself back on
		// track.
		floatVal = 0.0f;
		doubleVal = 0.0;
		vec2Val = Vector2.zero;
		vec3Val = Vector3.zero;
		vec4Val = Vector4.zero;
		queue.Update(0.2);
		// Completes the offset
		AreEqual(floatVal, floatEnd * 0.5f + floatStart * 0.5f, 0.01);
		AreEqual(doubleVal, doubleEnd * 0.5 + doubleStart * 0.5, 0.01);
		AreEqual(vec2Val, vec2End * 0.5f + vec2Start * 0.5f, 0.01f);
		AreEqual(vec3Val, vec3End * 0.5f + vec3Start * 0.5f, 0.01f);
		AreEqual(vec4Val, vec4End * 0.5f + vec4Start * 0.5f, 0.01f);
		queue.Update(0.5);
		
		AreEqual(floatVal, floatStart, 0.01f);
		AreEqual(doubleVal, doubleStart, 0.01);
		AreEqual(vec2Val, vec2Start, 0.01f);
		AreEqual(vec3Val, vec3Start, 0.01f);
		AreEqual(vec4Val, vec4Start, 0.01f);
		
		queue.Update(0.0);
		queue.Update(0.5);
		// Check that it does jump on repeat
		AreEqual(floatVal, floatEnd * 0.5f + floatStart * 0.5f, 0.01);
		AreEqual(doubleVal, doubleEnd * 0.5 + doubleStart * 0.5, 0.01);
		AreEqual(vec2Val, vec2End * 0.5f + vec2Start * 0.5f, 0.01f);
		AreEqual(vec3Val, vec3End * 0.5f + vec3Start * 0.5f, 0.01f);
		AreEqual(vec4Val, vec4End * 0.5f + vec4Start * 0.5f, 0.01f);
		
		Rect rectEnd = new Rect(-1.0f, 1.0f, 5.0f, 5.0f);
		Rect rectStart = new Rect(0.0f,2.0f, 6.0f, 6.0f);
		Rect rectVal = rectStart;
		Ref<Rect> rectRef = new Ref<Rect>(
			() =>  rectVal,
			t => rectVal = t
		);
		
		Vector2 firstAnchor = new Vector2(0.0f, 0.0f);
		Vector2 secondAnchor = new Vector2(1.0f, 0.0f);
		Vector3 thirdAnchor = new Vector2(0.0f, 1.0f);
		Vector2 forthAnchor = new Vector2(1.0f, 1.0f);
		
		queue = new CommandQueue();
		queue.Enqueue(
			Commands.ChangeFrom(rectRef, rectEnd, 1.0, firstAnchor),
			Commands.WaitForFrames(1),
			Commands.ChangeFrom(rectRef, rectEnd, 1.0, secondAnchor),
			Commands.WaitForFrames(1),
			Commands.ChangeFrom(rectRef, rectEnd, 1.0, thirdAnchor),
			Commands.WaitForFrames(1),
			Commands.ChangeFrom(rectRef, rectEnd, 1.0, forthAnchor)
		);
		
		// Test the top left corner.
		queue.Update(0.5);
		AreEqual(rectVal, new Rect(
			-0.5f, 1.5f, 
			(rectStart.width + rectEnd.width) * 0.5f,
			(rectStart.height + rectEnd.height) * 0.5f), 0.001f
		);
		queue.Update(0.5);
		AreEqual(rectVal, rectStart, 0.001f);
		queue.Update(0.0f);
		
		// Test the top right corner.
		queue.Update(0.7);
		AreEqual(rectVal, new Rect(
			5.4f - 5.7f, 1.7f, 
			rectStart.width * 0.7f + rectEnd.width * 0.3f,
			rectStart.height * 0.7f + rectEnd.height * 0.3f), 0.001f
		);
		queue.Update(0.3);
		AreEqual(rectVal, rectStart, 0.001f);
		queue.Update(0.0f);
		
		// Test the bottom left corner.
		queue.Update(0.6);
		AreEqual(rectVal, new Rect(
			-0.4f, 7.2f - 5.6f, 
			rectStart.width * 0.6f + rectEnd.width * 0.4f,
			rectStart.height * 0.6f + rectEnd.height * 0.4f), 0.001f
		);
		queue.Update(0.4);
		AreEqual(rectVal, rectStart, 0.001f); 
		queue.Update(0.0);
		
		// Test the bottom right corner.
		queue.Update(0.6);
		AreEqual(rectVal, new Rect(
			5.2f - 5.6f, 7.2f - 5.6f, 
			rectStart.width * 0.6f + rectEnd.width * 0.4f,
			rectStart.height * 0.6f + rectEnd.height * 0.4f), 0.001f
		);
		queue.Update(0.4);
		AreEqual(rectVal, rectStart, 0.001f);
		queue.Update(0.0f);
	}

	[Test]
	public static void TestRotateBy()
	{
		Quaternion quatStart = Quaternion.Euler(10.0f, 20.0f, 30.0f);
		Quaternion quatOffset = Quaternion.Euler(30.0f,20.0f,10.0f);
		Quaternion quatVal = quatStart;
		Ref<Quaternion> quatRef = new Ref<Quaternion>(
			() => quatVal,
			t => quatVal = t
		);
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.RotateBy(quatRef, quatOffset, 1.0)
			)
		);
		
		queue.Update(0.5f);
		AreEqual(quatVal, Quaternion.Slerp(quatStart, quatStart * quatOffset, 0.5f), 0.000001f);
		
		quatVal = Quaternion.identity;
		queue.Update(0.5f);
		AreEqual(quatVal, Quaternion.Slerp(Quaternion.identity, quatOffset,  0.5f), 0.000001f);
		queue.Update(0.5f);
		AreEqual(quatVal, quatOffset, 0.001f);
		queue.Update(0.5f);
		AreEqual(quatVal, quatOffset * Quaternion.Slerp(Quaternion.identity, quatOffset,  0.5f), 0.000001f);

		queue = new CommandQueue();
		
		// Make sure the rotation ends in the correct position when given a complex easing function.
		quatVal = Quaternion.identity;		
		queue.Enqueue(
			Commands.RotateBy(quatRef, quatOffset, 1f, Ease.OutElastic())
		);
		
		while (!queue.Update(1 / 30f)) {}

		AreEqual(quatVal, quatOffset, 0.001f);
	}

	[Test]
	public static void TestRotateTo()
	{
		Quaternion quatStart = Quaternion.Euler(10.0f, 20.0f, 30.0f);
		Quaternion quatEnd = Quaternion.Euler(30.0f,20.0f,10.0f);
		Quaternion quatVal = quatStart;
		Ref<Quaternion> quatRef = new Ref<Quaternion>(
			() => quatVal,
			t => quatVal = t
		);
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.RotateTo(quatRef, quatEnd, 1.0)
			)
		);
		
		queue.Update(0.5);
		AreEqual(quatVal, Quaternion.Slerp(quatStart, quatEnd, 0.5f), 0.000001f);
		
		quatVal = Quaternion.identity;
		queue.Update(0.5);
		AreEqual(quatVal, quatEnd, 0.000001f);
		queue.Update(0.5);
		AreEqual(quatVal, quatEnd, 0.000001f);
		
		// Make sure the rotation ends in the correct position when given a complex easing function.
		queue = new CommandQueue();

		quatVal = quatStart;		
		queue.Enqueue(
			Commands.RotateTo(quatRef, quatEnd, 1f, Ease.OutElastic())
		);

		while (!queue.Update(1 / 30f)) {}

		AreEqual(quatVal, quatEnd, 0.001f);
	}

	[Test]
	public static void TestRotateFrom()
	{
		Quaternion quatStart = Quaternion.Euler(10.0f, 20.0f, 30.0f);
		Quaternion quatEnd = Quaternion.Euler(30.0f,20.0f,10.0f);
		Quaternion quatVal = quatStart;
		Ref<Quaternion> quatRef = new Ref<Quaternion>(
			() => quatVal,
			t => quatVal = t
		);
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.Sequence(
					Commands.RotateFrom(quatRef, quatEnd, 1.0),
					Commands.WaitForFrames(1)
				)
			)
		);
		
		queue.Update(0.5);
		AreEqual(quatVal, Quaternion.Slerp(quatEnd, quatStart, 0.5f), 0.000001f);
		
		quatVal = Quaternion.identity;
		queue.Update(0.5);
		AreEqual(quatVal, quatStart, 0.000001f);
		queue.Update(0.0);
		queue.Update(0.5);
		AreEqual(quatVal, Quaternion.Slerp(quatEnd, quatStart, 0.5f), 0.000001f);

		// Make sure the rotation ends in the correct position when given a complex easing function.
		queue = new CommandQueue();

		quatVal = quatStart;		
		queue.Enqueue(
			Commands.RotateFrom(quatRef, quatEnd, 1f, Ease.OutElastic())
		);

		while (!queue.Update(1 / 30f)) {}

		AreEqual(quatVal, quatStart, 0.001f);
	}

	[Test]
	public static void TestScaleBy()
	{
		const float floatScale = 4.8f;
		const float floatStart = 1.2f;
		float floatVal = floatStart;
		Ref<float> floatRef = new Ref<float>(
			() => floatVal,
			t => floatVal = t
		);
		
		const double doubleScale = 3.2;
		const double doubleStart = 9.2;
		double doubleVal = doubleStart;
		Ref<double> doubleRef = new Ref<double>(
			() => doubleVal,
			t => doubleVal = t
		);
		
		Vector2 vec2Scale = new Vector2(9.5f, 2.0f);
		Vector2 vec2Start = new Vector2(4.0f, 5.0f);
		Vector2 vec2Val = vec2Start;
		Ref<Vector2> vec2Ref = new Ref<Vector2>(
			() => vec2Val,
			t =>  vec2Val = t
		);
		
		Vector3 vec3Scale = new Vector3(4.0f, 19.0f, 2.0f);
		Vector3 vec3Start = new Vector3(92.0f, 0.5f, 34.0f);
		Vector3 vec3Val = vec3Start;
		Ref<Vector3> vec3Ref = new Ref<Vector3>(
			() =>  vec3Val,
			t => vec3Val = t
		);
		
		Vector4 vec4Scale = new Vector4(92.0f, 0.5f, 14.0f, 7.0f);
		Vector4 vec4Start = new Vector4(0.4f, 10.0f, 3.0f, 82.0f);
		Vector4 vec4Val = vec4Start;
		Ref<Vector4> vec4Ref = new Ref<Vector4>(
			() => vec4Val,
			t => vec4Val = t
		);
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.Sequence(
					Commands.Parallel(
						Commands.ScaleBy(floatRef, floatScale, 1.0), 
						Commands.ScaleBy(doubleRef, doubleScale, 1.0), 
						Commands.ScaleBy(vec2Ref, vec2Scale, 1.0), 
						Commands.ScaleBy(vec3Ref, vec3Scale, 1.0), 
						Commands.ScaleBy(vec4Ref, vec4Scale, 1.0) 
					),
					Commands.WaitForFrames(1)
				)
			)
		);
		
		queue.Update(0.2f);
		
		Vector2 vec2ExpectedScale = vec2Scale;
		Vector3 vec3ExpectedScale = vec3Scale;
		Vector4 vec4ExpectedScale = vec4Scale;
		vec2ExpectedScale.Scale(new Vector2(0.2f, 0.2f));
		vec3ExpectedScale.Scale(new Vector3(0.2f, 0.2f, 0.2f));
		vec4ExpectedScale.Scale(new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
		vec2ExpectedScale += new Vector2(0.8f, 0.8f);
		vec3ExpectedScale += new Vector3(0.8f, 0.8f, 0.8f);
		vec4ExpectedScale += new Vector4(0.8f, 0.8f, 0.8f, 0.8f);
		vec2ExpectedScale.Scale(vec2Start);
		vec3ExpectedScale.Scale(vec3Start);
		vec4ExpectedScale.Scale(vec4Start);
		AreEqual(floatVal, floatStart * (0.8f + floatScale * 0.2f), 0.001f);
		AreEqual(doubleVal, doubleStart * (0.8 + doubleScale * 0.2), 0.001f);
		AreEqual(vec2Val, vec2ExpectedScale, 0.001f);
		AreEqual(vec3Val, vec3ExpectedScale, 0.001f);
		AreEqual(vec4Val, vec4ExpectedScale, 0.001f);
		
		queue.Update(0.8);
		vec2ExpectedScale = vec2Scale;
		vec3ExpectedScale = vec3Scale;
		vec4ExpectedScale = vec4Scale;
		vec2ExpectedScale.Scale(vec2Start);
		vec3ExpectedScale.Scale(vec3Start);
		vec4ExpectedScale.Scale(vec4Start);
		AreEqual(floatVal,  floatStart * floatScale, 0.001f);
		AreEqual(doubleVal,  doubleStart * doubleScale, 0.001f);
		AreEqual(vec2Val, vec2ExpectedScale, 0.001f);
		AreEqual(vec3Val, vec3ExpectedScale, 0.001f);
		AreEqual(vec4Val, vec4ExpectedScale, 0.001f);
		
		floatVal = floatStart;
		doubleVal = doubleStart;
		vec2Val = vec2Start;
		vec3Val = vec3Start;
		vec4Val = vec4Start;
		queue.Update(0.0);
		queue.Update(0.5);
		vec2ExpectedScale = vec2Scale;
		vec3ExpectedScale = vec3Scale;
		vec4ExpectedScale = vec4Scale;
		vec2ExpectedScale.Scale(new Vector2(0.5f, 0.5f));
		vec3ExpectedScale.Scale(new Vector3(0.5f, 0.5f, 0.5f));
		vec4ExpectedScale.Scale(new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
		vec2ExpectedScale += new Vector2(0.5f, 0.5f);
		vec3ExpectedScale += new Vector3(0.5f, 0.5f, 0.5f);
		vec4ExpectedScale += new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
		vec2ExpectedScale.Scale(vec2Start);
		vec3ExpectedScale.Scale(vec3Start);
		vec4ExpectedScale.Scale(vec4Start);
		AreEqual(floatVal, floatStart * (0.5f + floatScale * 0.5f), 0.001f);
		AreEqual(doubleVal, doubleStart * (0.5 + doubleScale * 0.5), 0.001f);
		AreEqual(vec2Val, vec2ExpectedScale, 0.001f);
		AreEqual(vec3Val, vec3ExpectedScale, 0.001f);
		AreEqual(vec4Val, vec4ExpectedScale, 0.001f);
	}

	[Test]
	public static void TestTintBy()
	{
		
		Color colourStart = new Color(0.4f, 0.2f, 0.7f, 0.5f);
		Color colourOffset = new Color(0.3f, 0.4f, 0.15f, 0.25f);
		Color colourVal = colourStart;
		Ref<Color> colourRef = new Ref<Color>(
			() => colourVal,
			t => colourVal = t
		);
		
		CommandQueue queue = new CommandQueue();
		
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.TintBy(colourRef, colourOffset, 1.0)
			)
		);
		
		queue.Update(0.5);
		AreEqual(colourVal, new Color(0.55f, 0.4f, 0.775f, 0.625f), 0.001f);
		colourVal = colourStart;
		queue.Update(0.5);
		AreEqual(colourVal, new Color(0.55f, 0.4f, 0.775f, 0.625f), 0.001f);
		queue.Update(0.5);
		AreEqual(colourVal, new Color(0.7f, 0.6f, 0.85f, 0.75f), 0.001f);
	}

	[Test]
	public static void TestTintTo()
	{
		Color colourStart = new Color(0.4f, 0.2f, 0.7f, 0.5f);
		Color colourEnd = new Color(0.3f, 0.4f, 0.15f, 0.25f);
		Color colourVal = colourStart;
		Ref<Color> colourRef = new Ref<Color>(
			() => colourVal,
			t => colourVal = t
		);
		
		CommandQueue queue = new CommandQueue();
		
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.TintTo(colourRef, colourEnd, 1.0)
			)
		);
		
		queue.Update(0.2);
		AreEqual(colourVal, colourStart * 0.8f + colourEnd * 0.2f, 0.001f);
		colourVal = colourStart;
		queue.Update(0.8);
		AreEqual(colourVal, colourEnd, 0.001f);
		queue.Update(0.5);
		AreEqual(colourVal, colourEnd, 0.001f);
	}

	[Test]
	public static void TestTintFrom()
	{
		Color colourStart = new Color(0.4f, 0.2f, 0.7f, 0.5f);
		Color colourEnd = new Color(0.3f, 0.4f, 0.15f, 0.25f);
		Color colourVal = colourStart;
		Ref<Color> colourRef = new Ref<Color>(
			() => colourVal,
			t => colourVal = t
		);
		
		CommandQueue queue = new CommandQueue();
		
		queue.Enqueue(
			Commands.Repeat(2,
				Commands.Sequence(
					Commands.TintFrom(colourRef, colourEnd, 1.0),
					Commands.WaitForFrames(1)
				)
			)
		);
		
		queue.Update(0.2);
		AreEqual(colourVal, colourStart * 0.2f + colourEnd * 0.8f, 0.001f);
		colourVal = colourStart;
		queue.Update(0.8);
		AreEqual(colourVal, colourStart, 0.001f);
		queue.Update(0.0);
		queue.Update(0.5);
		AreEqual(colourVal, colourStart * 0.5f + colourEnd * 0.5f, 0.001f);
		
	}
	
}

}

