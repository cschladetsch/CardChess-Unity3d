using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class CanvasGroupExtensions
{
	public static Ref<float> ToAlphaRef(this CanvasGroup group)
	{
		if (group == null) {
			throw new ArgumentNullException("group");
		}

		return new Ref<float>(
			() => group.alpha,
			(t) => group.alpha = t
		);
	}
}

}
