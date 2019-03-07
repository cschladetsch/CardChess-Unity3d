using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class RectTransformExtensions
{
	#region Extension methods

	public static Ref<Vector2> ToOffsetMinRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);
		return new Ref<Vector2>(
			() => transform.offsetMin,
			(t) => transform.offsetMin = t
		);
	}
	
	public static Ref<Vector2> ToOffsetMaxRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<Vector2>(
			() => transform.offsetMax,
			(t) => transform.offsetMax = t
		);
	}

	public static Ref<Vector2> ToAnchorMinRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<Vector2>(
			() => transform.anchorMin,
			(t) => transform.anchorMin = t
		);
	}
	
	public static Ref<Vector2> ToAnchorMaxRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<Vector2>(
			() => transform.anchorMax,
			(t) => transform.anchorMax = t
		);
	}
	
	public static Ref<Vector2> ToAnchorRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<Vector2>(
			() => transform.anchoredPosition,
			(t) => transform.anchoredPosition = t
		);
	}
	
	public static Ref<Vector2> ToSizeDeltaRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<Vector2>(
			() => transform.sizeDelta,
			(t) => transform.sizeDelta = t
		);
	}

	public static Ref<Vector2> ToSizeRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<Vector2>(
			() => transform.rect.size,
			t => {
				transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t.x);
				transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, t.y);
			}
		);
	}

	public static Ref<float> ToEdgeInsetRef(this RectTransform transform, RectTransform.Edge edge)
	{
		CheckTransformNonNull(transform);

		return new Ref<float>(
			() => {
				switch (edge) {
					case RectTransform.Edge.Bottom:
						return transform.offsetMin.y;
					case RectTransform.Edge.Left:
						return transform.offsetMin.x;
					case RectTransform.Edge.Right:
						return transform.offsetMax.x;
					case RectTransform.Edge.Top:
						return transform.offsetMax.y;
				}
				return 0f;
			},
			t => {
				var size = transform.rect.size;
				float lengthSize = size.x;
				if (edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Bottom) {
					lengthSize = size.y;
				}
				transform.SetInsetAndSizeFromParentEdge(edge, t, lengthSize);
			}
		);
	}

	public static Ref<Vector2> ToBottomLeftRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		var leftRef = transform.ToEdgeInsetRef(RectTransform.Edge.Left);
		var bottomRef = transform.ToEdgeInsetRef(RectTransform.Edge.Bottom);
		return new Ref<Vector2>(
			() => new Vector2(leftRef.Value, bottomRef.Value) ,
			t => {
				leftRef.Value = t.x;
				bottomRef.Value = t.y;
			}
		);
	}

	public static Ref<Vector2> ToTopLeftRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		var leftRef = transform.ToEdgeInsetRef(RectTransform.Edge.Left);
		var topRef = transform.ToEdgeInsetRef(RectTransform.Edge.Top);
		return new Ref<Vector2>(
			() => new Vector2(leftRef.Value, topRef.Value),
			t => {
				leftRef.Value = t.x;
				topRef.Value = t.y;
			}
		);
	}

	public static Ref<Vector2> ToTopRightRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		var rightRef = transform.ToEdgeInsetRef(RectTransform.Edge.Right);
		var topRef = transform.ToEdgeInsetRef(RectTransform.Edge.Top);
		return new Ref<Vector2>(
			() => new Vector2(rightRef.Value, topRef.Value),
			t => {
				rightRef.Value = t.x;
				topRef.Value = t.y;
			}
		);
	}

	public static Ref<Vector2> ToBottomRightRef(this RectTransform transform)
	{
		CheckTransformNonNull(transform);

		var rightRef = transform.ToEdgeInsetRef(RectTransform.Edge.Right);
		var bottomRef = transform.ToEdgeInsetRef(RectTransform.Edge.Bottom);
		return new Ref<Vector2>(
			() => new Vector2(rightRef.Value, bottomRef.Value),
			t => {
				rightRef.Value = t.x;
				bottomRef.Value = t.y;
			}
		);
	}

	#endregion

	#region Private methods

	private static void CheckTransformNonNull(RectTransform transform)
	{
		if (transform == null) {
			throw new ArgumentNullException("transform");
		}
	}

	#endregion
}

}