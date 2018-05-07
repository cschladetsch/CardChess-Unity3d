using UnityEngine;
using System;

namespace CoLib
{

public static class GUITextureExtensions 
{
	#region Extension methods

	public static Ref<Color> ToColorRef(this GUITexture texture)
	{
		CheckTextureNonNull(texture);
		return new Ref<Color>(
			() => texture.color,
			t => texture.color = t
		);
	}

	public static Ref<float> ToRedRef(this GUITexture texture)
	{
		CheckTextureNonNull(texture);

		return new Ref<float>(
			() => texture.color.r,
			t => {
				var color = texture.color;
				color.r = t;
				texture.color = color;
			}
		);
	}

	public static Ref<float> ToGreenRef(this GUITexture texture)
	{
		CheckTextureNonNull(texture);

		return new Ref<float>(
			() => texture.color.g,
			t => {
				var color = texture.color;
				color.g = t;
				texture.color = color;
			}
		);
	}

	public static Ref<float> ToBlueRef(this GUITexture texture)
	{
		CheckTextureNonNull(texture);

		return new Ref<float>(
			() => texture.color.b,
			t => {
				var color = texture.color;
				color.b = t;
				texture.color = color;
			}
		);
	}

	public static Ref<float> ToAlphaRef(this GUITexture texture)
	{
		CheckTextureNonNull(texture);

		return new Ref<float>(
			() => texture.color.a,
			t => {
				var color = texture.color;
				color.a = t;
				texture.color = color;
			}
		);
	}

	#endregion

	#region Private methods

	private static void CheckTextureNonNull(GUITexture texture)
	{
		if (texture == null) {
			throw new ArgumentNullException("texture");
		}
	}

	#endregion
}

}
