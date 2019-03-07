using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class LayoutElementExtensions
{
	#region Extension methods

	public static Ref<float> ToMinWidthRef(this LayoutElement layoutElement)
	{
		CheckLayoutElementNonNull(layoutElement);
		return new Ref<float>(
			() => layoutElement.minWidth,
			t => layoutElement.minWidth = t
		);
	}

	public static Ref<float> ToMinHeightRef(this LayoutElement layoutElement)
	{
		CheckLayoutElementNonNull(layoutElement);

		return new Ref<float>(
			() => layoutElement.minHeight,
			t => layoutElement.minHeight = t
		);
	}

	public static Ref<float> ToPreferredWidthRef(this LayoutElement layoutElement)
	{
		CheckLayoutElementNonNull(layoutElement);

		return new Ref<float>(
			() => layoutElement.preferredWidth,
			t => layoutElement.preferredWidth = t
		);
	}

	public static Ref<float> ToPreferredHeightRef(this LayoutElement layoutElement)
	{
		CheckLayoutElementNonNull(layoutElement);

		return new Ref<float>(
			() => layoutElement.preferredHeight,
			t => layoutElement.preferredHeight = t
		);
	}

	public static Ref<float> ToFlexibleWidthRef(this LayoutElement layoutElement)
	{
		CheckLayoutElementNonNull(layoutElement);

		return new Ref<float>(
			() => layoutElement.flexibleWidth,
			t => layoutElement.flexibleWidth = t
		);
	}

	public static Ref<float> ToFlexibleHeightRef(this LayoutElement layoutElement)
	{
		CheckLayoutElementNonNull(layoutElement);

		return new Ref<float>(
			() => layoutElement.flexibleHeight,
			t => layoutElement.flexibleHeight = t
		);
	}

	#endregion

	#region Private methods

	private static void CheckLayoutElementNonNull(LayoutElement layoutElement)
	{
		if (layoutElement == null) {
			throw new ArgumentNullException("layoutElement");
		}
	}

	#endregion
}

}
