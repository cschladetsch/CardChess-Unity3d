using UnityEngine;
using System.Collections;

#pragma warning disable CS0618 

using UnityEngine.UI;
	
namespace CoLib
{

public static partial class Commands
{
	#region MoveTo
	
	public static CommandDelegate MoveTo(GameObject gameObject, Vector3 endPosition, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");
		return MoveTo(gameObject.transform, endPosition, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate MoveTo(Transform transform, Vector3 endPosition, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return ChangeTo(transform.ToPositionRef(isLocalSpace), endPosition, duration, ease);
	}
	
	#endregion
	
	#region MoveBy

	public static CommandDelegate MoveBy(GameObject gameObject, Vector3 offset, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return MoveBy(gameObject.transform, offset, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate MoveBy(Transform transform, Vector3 offset, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return ChangeBy(transform.ToPositionRef(isLocalSpace),  offset, duration, ease);
	}
	
	#endregion
	
	#region MoveFrom
	
	public static CommandDelegate MoveFrom(GameObject gameObject, Vector3 startPosition, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return MoveFrom(gameObject.transform, startPosition, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate MoveFrom(Transform transform, Vector3 startPosition, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return ChangeFrom(transform.ToPositionRef(isLocalSpace), startPosition, duration, ease);
	}

	#endregion

	#region MoveFromOffset
	
	public static CommandDelegate MoveFromOffset(GameObject gameObject, Vector3 offset, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return MoveFromOffset(gameObject.transform, offset, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate MoveFromOffset(Transform transform, Vector3 offset, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return ChangeFromOffset(transform.ToPositionRef(isLocalSpace), offset, duration, ease);
	}

	#endregion

	#region RotateTo
	
	public static CommandDelegate RotateTo(GameObject gameObject, Quaternion endRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return RotateTo(gameObject.transform, endRotation, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate RotateTo(Transform transform, Quaternion endRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return RotateTo(transform.ToRotationRef(isLocalSpace), endRotation, duration, ease);
	}
	
	#endregion
	
	#region RotateBy
	
	public static CommandDelegate RotateBy(GameObject gameObject, Quaternion endRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return RotateBy(gameObject.transform, endRotation, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate RotateBy(Transform transform, Quaternion endRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return RotateBy(transform.ToRotationRef(isLocalSpace), endRotation, duration, ease);
	}
	
	#endregion
	
	#region RotateFrom
	
	public static CommandDelegate RotateFrom(GameObject gameObject, Quaternion startRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return RotateFrom(gameObject.transform, startRotation, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate RotateFrom(Transform transform, Quaternion startRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return RotateFrom(transform.ToRotationRef(isLocalSpace), startRotation, duration, ease);
	}
	
	#endregion

	#region RotateFromOffset
	
	public static CommandDelegate RotateFromOffset(GameObject gameObject, Quaternion offset, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return RotateFromOffset(gameObject.transform, offset, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate RotateFromOffset(Transform transform, Quaternion offset, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return RotateFromOffset(transform.ToRotationRef(isLocalSpace), offset, duration, ease);
	}
	
	#endregion
	
	#region ScaleTo
	
	public static CommandDelegate ScaleTo(GameObject gameObject, float endScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleTo(gameObject.transform, endScale, duration, ease);
	}
	
	public static CommandDelegate ScaleTo(GameObject gameObject, Vector3 endScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleTo(gameObject.transform, endScale, duration, ease);
	}
	
	public static CommandDelegate ScaleTo(Transform transform, float endScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");

		return ScaleTo(transform, Vector3.one * endScale, duration, ease);
	}
	
	public static CommandDelegate ScaleTo(Transform transform, Vector3 endScale, double duration, CommandEase ease = null)
	{	
		CheckArgumentNonNull(transform, "transform");
	
		return ChangeTo(transform.ToScaleRef(), endScale, duration, ease);
	}
	
	#endregion
	
	#region ScaleBy
	
	public static CommandDelegate ScaleBy(GameObject gameObject, float scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleBy(gameObject.transform, scaleFactor, duration, ease);
	}
	
	public static CommandDelegate ScaleBy(GameObject gameObject, Vector3 scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleBy(gameObject.transform, scaleFactor, duration, ease);
	}
	
	public static CommandDelegate ScaleBy(Transform transform, float scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");

		return ScaleBy(transform, Vector3.one * scaleFactor, duration, ease);
	}
	
	public static CommandDelegate ScaleBy(Transform transform, Vector3 scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");
		
		return ScaleBy(transform.ToScaleRef(), scaleFactor, duration, ease);
	}
	
	#endregion
	
	#region ScaleFrom
	
	public static CommandDelegate ScaleFrom(GameObject gameObject, float startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleFrom(gameObject.transform, startScale, duration, ease);
	}
	
	public static CommandDelegate ScaleFrom(GameObject gameObject, Vector3 startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleFrom(gameObject.transform, startScale, duration, ease);
	}
	
	public static CommandDelegate ScaleFrom(Transform transform, float startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");

		return ScaleFrom(transform, Vector3.one * startScale, duration, ease);
	}
	
	public static CommandDelegate ScaleFrom(Transform transform, Vector3 startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");
		
		return ChangeFrom(transform.ToScaleRef(), startScale, duration, ease);
	}
	
	#endregion

	#region ScaleFromOffset
	
	public static CommandDelegate ScaleFromOffset(GameObject gameObject, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleFromOffset(gameObject.transform, offset, duration, ease);
	}
	
	public static CommandDelegate ScaleFromOffset(GameObject gameObject, Vector3 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleFromOffset(gameObject.transform, offset, duration, ease);
	}
	
	public static CommandDelegate ScaleFromOffset(Transform transform, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");

		return ScaleFromOffset(transform, Vector3.one * offset, duration, ease);
	}
	
	public static CommandDelegate ScaleFromOffset(Transform transform, Vector3 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");
		
		return ChangeFromOffset(transform.ToScaleRef(), offset, duration, ease);
	}
	
	#endregion
	
	#region TintTo
	
	public static CommandDelegate TintTo(GameObject gameObject, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return TintTo(gameObject.GetComponent<Renderer>().material, endColour, duration, ease);
	}
	
	public static CommandDelegate TintTo(Renderer renderer, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintTo(renderer.material, endColour, duration, ease);
	}
	
	public static CommandDelegate TintTo(Material material, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		return TintTo(material.ToColorRef(), endColour, duration, ease);
	}
	
	public static CommandDelegate TintTo(SpriteRenderer renderer, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintTo(renderer.ToColorRef(), endColour, duration, ease);
	}
		
	public static CommandDelegate TintTo(Image texture, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		return TintTo(texture.ToColorRef(), endColour, duration, ease);
	}

	public static CommandDelegate TintTo(Text text, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		return TintTo(text.ToColorRef(), endColour, duration, ease);
	}
	
	#endregion
	
	#region TintBy
	
	public static CommandDelegate TintBy(GameObject gameObject, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return TintBy(gameObject.GetComponent<Renderer>().material, offset, duration, ease);
	}
	
	public static CommandDelegate TintBy(Renderer renderer, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintBy(renderer.material, offset, duration, ease);
	}
	
	public static CommandDelegate TintBy(Material material, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");
		return TintBy(material.ToColorRef(), offset, duration, ease);
	}
	
	public static CommandDelegate TintBy(SpriteRenderer renderer, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintBy(renderer.ToColorRef(), offset, duration, ease);
	}
		
	public static CommandDelegate TintBy(Image texture, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		return TintBy(texture.ToColorRef(), offset, duration, ease);
	}

	public static CommandDelegate TintBy(Text text, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		return TintBy(text.ToColorRef(), offset, duration, ease);
	}
	
	#endregion
	
	#region TintFrom
	
	public static CommandDelegate TintFrom(GameObject gameObject, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return TintFrom(gameObject.GetComponent<Renderer>().material, startColour, duration, ease);
	}
	
	public static CommandDelegate TintFrom(Renderer renderer, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintFrom(renderer.material, startColour, duration, ease);
	}
	
	public static CommandDelegate TintFrom(Material material, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		return TintFrom(material.ToColorRef(), startColour, duration, ease);
	}
	
	public static CommandDelegate TintFrom(SpriteRenderer renderer, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintFrom(renderer.ToColorRef(), startColour, duration, ease);
	}
		
	public static CommandDelegate TintFrom(Image texture, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		return TintFrom(texture.ToColorRef(), startColour, duration, ease);
	}

	public static CommandDelegate TintFrom(Text text, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		return TintFrom(text.ToColorRef(), startColour, duration, ease);
	}
	
	#endregion
		
	#region AlphaTo
	
	public static CommandDelegate AlphaTo(GameObject gameObject, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return AlphaTo(gameObject.GetComponent<Renderer>().material, endAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaTo(Renderer renderer, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return AlphaTo(renderer.material, endAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaTo(Material material, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		return ChangeTo(material.ToAlphaRef(), endAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaTo(SpriteRenderer renderer, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return ChangeTo(renderer.ToAlphaRef(), endAlpha, duration, ease);
	}
		
	public static CommandDelegate AlphaTo(Image texture, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		return ChangeTo(texture.ToAlphaRef(), endAlpha, duration, ease);
	}

	public static CommandDelegate AlphaTo(Text text, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		return ChangeTo(text.ToAlphaRef(), endAlpha, duration, ease);
	}
	
	#endregion
		
	#region AlphaBy
	
	public static CommandDelegate AlphaBy(GameObject gameObject, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return AlphaBy(gameObject.GetComponent<Renderer>().material, offset, duration, ease);
	}
	
	public static CommandDelegate AlphaBy(Renderer renderer, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return AlphaBy(renderer.material, offset, duration, ease);
	}
	
	public static CommandDelegate AlphaBy(Material material, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		return ChangeBy(material.ToAlphaRef(), offset, duration, ease);
	}
	public static CommandDelegate AlphaBy(SpriteRenderer renderer, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return ChangeBy(renderer.ToAlphaRef(), offset, duration, ease);
	}
		
	public static CommandDelegate AlphaBy(Image texture, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		return ChangeBy(texture.ToAlphaRef(), offset, duration, ease);
	}

	public static CommandDelegate AlphaBy(Text text, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		return ChangeBy(text.ToAlphaRef(), offset, duration, ease);
	}
	
	#endregion

	#region AlphaFrom
	
	public static CommandDelegate AlphaFrom(GameObject gameObject, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return AlphaFrom(gameObject.GetComponent<Renderer>().material, startAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaFrom(Renderer renderer, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return AlphaFrom(renderer.material, startAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaFrom(Material material, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		return ChangeFrom(material.ToAlphaRef(), startAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaFrom(SpriteRenderer renderer, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return ChangeFrom(renderer.ToAlphaRef(), startAlpha, duration, ease);
	}
		
	public static CommandDelegate AlphaFrom(Image texture, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		return ChangeFrom(texture.ToAlphaRef(), startAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaFrom(Text text, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		return ChangeFrom(text.ToAlphaRef(), startAlpha, duration, ease);
	}

	#endregion

	#region AlphaFromOffset
	
	public static CommandDelegate AlphaFromOffset(GameObject gameObject, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return AlphaFromOffset(gameObject.GetComponent<Renderer>().material, offset, duration, ease);
	}
	
	public static CommandDelegate AlphaFromOffset(Renderer renderer, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return AlphaFromOffset(renderer.material, offset, duration, ease);
	}
	
	public static CommandDelegate AlphaFromOffset(Material material, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		return ChangeFromOffset(material.ToAlphaRef(), offset, duration, ease);
	}
	
	public static CommandDelegate AlphaFromOffset(SpriteRenderer renderer, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return ChangeFromOffset(renderer.ToAlphaRef(), offset, duration, ease);
	}
		
	public static CommandDelegate AlphaFromOffset(Image texture, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		return ChangeFromOffset(texture.ToAlphaRef(), offset, duration, ease);
	}
	
	public static CommandDelegate AlphaFromOffset(Text text, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		return ChangeFromOffset(text.ToAlphaRef(), offset, duration, ease);
	}

	#endregion
}
	
}