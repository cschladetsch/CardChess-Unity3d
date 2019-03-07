using UnityEngine;
using System;

namespace CoLib
{

public static class SpriteRendererExtensions 
{
	#region Extension methods

	public static Ref<Color> ToColorRef(this SpriteRenderer spriteRenderer)
	{
		CheckRendererNonNull(spriteRenderer);
		return new Ref<Color>(
			() => spriteRenderer.color,
			t => spriteRenderer.color = t
		);
	}
		public static Ref<float> ToRedRef(this SpriteRenderer spriteRenderer)
	{
		CheckRendererNonNull(spriteRenderer);

		return new Ref<float>(
			() => spriteRenderer.color.r,
			t => {
				var color = spriteRenderer.color;
				color.r = t;
				spriteRenderer.color = color;
			}
		);
	}

	public static Ref<float> ToGreenRef(this SpriteRenderer spriteRenderer)
	{
		CheckRendererNonNull(spriteRenderer);

		return new Ref<float>(
			() => spriteRenderer.color.g,
			t => {
				var color = spriteRenderer.color;
				color.g = t;
				spriteRenderer.color = color;
			}
		);
	}

	public static Ref<float> ToBlueRef(this SpriteRenderer spriteRenderer)
	{
		CheckRendererNonNull(spriteRenderer);

		return new Ref<float>(
			() => spriteRenderer.color.b,
			t => {
				var color = spriteRenderer.color;
				color.b = t;
				spriteRenderer.color = color;
			}
		);
	}

	public static Ref<float> ToAlphaRef(this SpriteRenderer spriteRenderer)
	{
		CheckRendererNonNull(spriteRenderer);

		return new Ref<float>(
			() => spriteRenderer.color.a,
			t => {
				var color = spriteRenderer.color;
				color.a = t;
				spriteRenderer.color = color;
			}
		);
	}

	#endregion

	#region Private methods

	private static void CheckRendererNonNull(SpriteRenderer spriteRenderer)
	{
		if (spriteRenderer == null) {
			throw new ArgumentNullException("spriteRenderer");
		}
	}

	#endregion
}

}
