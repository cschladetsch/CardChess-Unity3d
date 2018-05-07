using System;
using UnityEngine;

namespace CoLib 
{

public static partial class Commands
{
	public static CommandDelegate Log(string text)
	{
		return Commands.Do( () => Debug.Log(text) );
	}

	public static CommandDelegate LogError(string text)
	{
		return Commands.Do (() => Debug.LogError(text));
	}

	public static CommandDelegate LogWarning(string text)
	{
		return Commands.Do (() => Debug.LogWarning (text));
	}

	public static CommandDelegate LogException(Exception e)
	{
		return Commands.Do (() => Debug.LogException (e));
	}

	public static CommandDelegate Enable(MonoBehaviour behaviour, bool isEnabled = true)
	{
		return Commands.Do (() => behaviour.enabled = isEnabled);
	}

	public static CommandDelegate SetActive(GameObject gm, bool isActive)
	{
		return Commands.Do (() => gm.SetActive (isActive));
	}

	public static CommandDelegate SendMessage(GameObject gm, string eventName, object obj  = null, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
	{
		return Commands.Do( () => gm.SendMessage (eventName, obj, options));
	}

}

}

