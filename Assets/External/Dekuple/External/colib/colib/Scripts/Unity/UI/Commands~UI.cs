using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{
	
public static partial class Commands
{
	#region Graphic

	public static CommandDelegate TintTo(Graphic graphic, Color endColor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(graphic, "graphic");
		return TintTo(graphic.ToColorRef(), endColor, duration, ease);
	}

	public static CommandDelegate TintFrom(Graphic graphic, Color startColor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(graphic, "graphic");
		return TintFrom(graphic.ToColorRef(), startColor, duration, ease);
	}

	public static CommandDelegate TintBy(Graphic graphic, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(graphic, "graphic");
		return TintBy(graphic.ToColorRef(), offset, duration, ease);
	}

	public static CommandDelegate AlphaTo(Graphic graphic, float alpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(graphic, "graphic");
		return AlphaTo(graphic.ToColorRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaFrom(Graphic graphic, float alpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(graphic, "graphic");
		return AlphaFrom(graphic.ToColorRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaBy(Graphic graphic, float alpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(graphic, "graphic");
		return AlphaBy(graphic.ToColorRef(), alpha, duration, ease);
	}
	public static CommandDelegate AlphaFromOffset(Graphic graphic, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(graphic, "graphic");
		return AlphaFromOffset(graphic.ToColorRef(), offset, duration, ease);
	}
	
	#endregion

	#region CanvasGroup

	public static CommandDelegate AlphaTo(CanvasGroup group, float alpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(group, "group");
		return ChangeTo(group.ToAlphaRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaFrom(CanvasGroup group, float alpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(group, "group");
		return ChangeFrom(group.ToAlphaRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaBy(CanvasGroup group, float alpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(group, "group");
		return ChangeBy(group.ToAlphaRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaFromOffset(CanvasGroup group, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(group, "group");
		return ChangeFromOffset(group.ToAlphaRef(), offset, duration, ease);
	}
	#endregion

	#region RectTransform

	public static CommandDelegate ScaleTo(RectTransform transform, Vector2 endScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");
		return ChangeTo(transform.ToScaleRef(), endScale, duration, ease);
	}

	public static CommandDelegate ScaleFrom(RectTransform transform, Vector2 startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");
		return ChangeFrom(transform.ToScaleRef(), startScale, duration, ease);
	}

	public static CommandDelegate ScaleBy(RectTransform transform, Vector2 scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");
		return ScaleBy(transform.ToScaleRef(), scaleFactor, duration, ease);
	}

	#endregion
}

}
