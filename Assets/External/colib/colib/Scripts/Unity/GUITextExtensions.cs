using UnityEngine;
using System;

namespace CoLib
{

public static class GUITextExtensions 
{
	#region Extension methods

	public static Ref<Color> ToColorRef(this GUIText text)
	{
		CheckTextNonNull(text);
		return new Ref<Color>(
			() => text.color,
			t => text.color = t
		);
	}
		public static Ref<float> ToRedRef(this GUIText text)
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

	public static Ref<float> ToGreenRef(this GUIText text)
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

	public static Ref<float> ToBlueRef(this GUIText text)
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

	public static Ref<float> ToAlphaRef(this GUIText text)
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

	private static void CheckTextNonNull(GUIText text)
	{
		if (text == null) {
			throw new ArgumentNullException("text");
		}
	}

	#endregion
}

}
