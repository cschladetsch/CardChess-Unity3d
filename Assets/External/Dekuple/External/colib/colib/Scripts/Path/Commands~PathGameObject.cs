using UnityEngine;

namespace CoLib
{
	
public static partial class Commands
{
	#region MoveAlong

	public static CommandDelegate MoveAlong(GameObject gameObject, IPath path, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");
		return MoveAlong(gameObject.transform, path, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate MoveAlong(Transform transform, IPath path, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return MoveAlong(transform.ToPositionRef(isLocalSpace), path, duration, ease);
	}

	#endregion

	#region LookAlong

	public static CommandDelegate LookAlong(GameObject gameObject, IPath path, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");
		return LookAlong(gameObject.transform, path, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate LookAlong(Transform transform, IPath path, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return LookAlong(transform.ToRotationRef(isLocalSpace), path, duration, ease);
	}

	#endregion

}

}