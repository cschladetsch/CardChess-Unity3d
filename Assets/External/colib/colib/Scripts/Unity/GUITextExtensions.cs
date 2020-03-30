using UnityEngine;
using System;

#pragma warning disable CS0618 

using UnityEngine.UI;

namespace CoLib
{

public static class TextExtensions 
{
	#region Extension methods

	public static Ref<Color> ToColorRef(this Text text)
	{
		CheckTextNonNull(text);
		return new Ref<Color>(
			() => text.color,
			t => text.color = t
		);
	}
		public static Ref<float> ToRedRef(this Text text)
	{
		CheckTextNonNull(text);

		return new Ref<float>(
			() => text.color.r,
			t => {
				var color = text.color;
				color.r = t;
				text.color = color;
			}
		);
	}

	public static Ref<float> ToGreenRef(this Text text)
	{
		CheckTextNonNull(text);

		return new Ref<float>(
			() => text.color.g,
			t => {
				var color = text.color;
				color.g = t;
				text.color = color;
			}
		);
	}

	public static Ref<float> ToBlueRef(this Text text)
	{
		CheckTextNonNull(text);

		return new Ref<float>(
			() => text.color.b,
			t => {
				var color = text.color;
				color.b = t;
				text.color = color;
			}
		);
	}

	public static Ref<float> ToAlphaRef(this Text text)
	{
		CheckTextNonNull(text);

		return new Ref<float>(
			() => text.color.a,
			t => {
				var color = text.color;
				color.a = t;
				text.color = color;
			}
		);
	}

	#endregion

	#region Private methods

	private static void CheckTextNonNull(Text text)
	{
		if (text == null) {
			throw new ArgumentNullException("text");
		}
	}

	#endregion
}

}
