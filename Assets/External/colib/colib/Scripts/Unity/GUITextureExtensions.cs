using UnityEngine;
using System;

#pragma warning disable CS0618 

using UnityEngine.UI;

namespace CoLib
{

public static class ImageExtensions 
{
	#region Extension methods

	public static Ref<Color> ToColorRef(this Image texture)
	{
		CheckTextureNonNull(texture);
		return new Ref<Color>(
			() => texture.color,
			t => texture.color = t
		);
	}

	public static Ref<float> ToRedRef(this Image texture)
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

	public static Ref<float> ToGreenRef(this Image texture)
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

	public static Ref<float> ToBlueRef(this Image texture)
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

	public static Ref<float> ToAlphaRef(this Image texture)
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

	private static void CheckTextureNonNull(Image texture)
	{
		if (texture == null) {
			throw new ArgumentNullException("texture");
		}
	}

	#endregion
}

}
