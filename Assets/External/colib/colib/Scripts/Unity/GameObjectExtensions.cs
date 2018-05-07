using System.Collections;
using System;
using UnityEngine;

namespace CoLib
{

public static class GameObjectExtensions
{
	#region Extension methods

	public static Ref<Vector3> ToPositionRef(this GameObject gameObject, bool isLocalSpace)
	{
		CheckArgumentNonNull(gameObject);
		return gameObject.transform.ToPositionRef(isLocalSpace);
	}

	public static Ref<Quaternion> ToRotationRef(this GameObject gameObject, bool isLocalSpace)
	{
		CheckArgumentNonNull(gameObject);

		return gameObject.transform.ToRotationRef(isLocalSpace);
	}

	public static Ref<Vector3> ToScaleRef(this GameObject gameObject)
	{
		CheckArgumentNonNull(gameObject);

		return gameObject.transform.ToScaleRef();
	}

	public static CommandQueue Queue(this GameObject gameObject, params CommandDelegate[] commands)
	{
		CheckArgumentNonNull(gameObject);

		return GetCommandBehaviour(gameObject).Queue(commands);
	}

	public static void RemoveQueue(this GameObject gameObject, CommandQueue queue)
	{
		CheckArgumentNonNull(gameObject);

		GetCommandBehaviour(gameObject).RemoveQueue(queue);
	}

	public static void Schedule(this GameObject gameObject, params CommandDelegate[] commands)
	{
		CheckArgumentNonNull(gameObject);

		GetCommandBehaviour(gameObject).Schedule(commands);
	}

	#endregion

	#region Private static methods

	private static void CheckArgumentNonNull(GameObject gameObject)
	{
		if (gameObject == null) {
			throw new ArgumentNullException("gameObject"); 
		}
	}

	private static CommandBehaviour GetCommandBehaviour(GameObject gm)
	{
		var behaviour = gm.GetComponent<CommandBehaviour>();
		if (behaviour == null) {
			behaviour = gm.AddComponent<CommandBehaviour> ();
		}
		return behaviour;
	}

	#endregion
}

}
