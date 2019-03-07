using UnityEngine;
using System;

namespace CoLib
{

public static class CameraExtensions 
{
	#region Extension methods

	public static Ref<float> ToFieldOfViewRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);
		return new Ref<float>(
			() => camera.fieldOfView,
			t => camera.fieldOfView = t
		);
	}

	public static Ref<float> ToAspectRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<float>(
			() => camera.aspect,
			t => camera.aspect = t
		);
	}

	public static Ref<float> ToNearClipPlaneRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<float>(
			() => camera.nearClipPlane,
			t => camera.nearClipPlane = t
		);
	}

	public static Ref<float> ToFarClipPlaneRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<float>(
			() => camera.farClipPlane,
			t => camera.farClipPlane = t
		);
	}

	public static Ref<float> ToOrthographicSize(this Camera camera)
	{
		CheckArgumentNonNull(camera);
		return new Ref<float>(
			() => camera.orthographicSize,
			t => camera.orthographicSize = t 
		);
	}

	public static Ref<Color> ToBackgroundColorRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<Color>(
			() => camera.backgroundColor,
			t => camera.backgroundColor = t
		);
	}

	public static Ref<float> ToBackgroundRedRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<float>(
			() => camera.backgroundColor.r,
			t => {
				var color = camera.backgroundColor;
				color.r = t;
				camera.backgroundColor = color;
			}
		);
	}

	public static Ref<float> ToBackgroundGreenRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<float>(
			() => camera.backgroundColor.g,
			t => {
				var color = camera.backgroundColor;
				color.g = t;
				camera.backgroundColor = color;
			}
		);
	}

	public static Ref<float> ToBackgroundBlueRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<float>(
			() => camera.backgroundColor.b,
			t => {
				var color = camera.backgroundColor;
				color.b = t;
				camera.backgroundColor = color;
			}
		);
	}

	public static Ref<float> ToBackgroundAlphaRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<float>(
			() => camera.backgroundColor.a,
			t => {
				var color = camera.backgroundColor;
				color.a = t;
				camera.backgroundColor = color;
			}
		);
	}

	public static Ref<Rect> ToPixelRectRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<Rect>(
			() => camera.pixelRect,
			t => camera.pixelRect = t
		);
	}

	public static Ref<Rect> ToRectRef(this Camera camera)
	{
		CheckArgumentNonNull(camera);

		return new Ref<Rect>(
			() => camera.rect,
			t => camera.rect = t
		);
	}


	#endregion

	#region Private static methods

	private static void CheckArgumentNonNull(Camera camera)
	{
		if (camera == null) {
			throw new ArgumentNullException("camera"); 
		}
	}

	#endregion
}

}
