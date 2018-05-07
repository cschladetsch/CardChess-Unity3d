using UnityEngine;

namespace CoLib
{
	
public static partial class Commands
{
	#region MoveAlong

	public static CommandDelegate MoveAlong(Ref<Vector2> position, IPath path, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(path, "path");
		CheckDurationGreaterThanOrEqualToZero(duration);
		Ref<Vector3> val = new Ref<Vector3>(
			() => position.Value,
			t => position.Value = t
		);

		return MoveAlong(val, path, duration, ease);
	}

	public static CommandDelegate MoveAlong(Ref<Vector3> position, IPath path, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(path, "path");
		CheckDurationGreaterThanOrEqualToZero(duration);

		return Commands.Duration( (t) => position.Value = path.GetPoint(t), duration, ease);
	}

	#endregion

	#region LookAlong

	public static CommandDelegate LookAlong(Ref<Quaternion> rotation, IPath path, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(path, "path");
		CheckDurationGreaterThanOrEqualToZero(duration);

		return Commands.Duration( (t) => rotation.Value = path.GetRotation(t), duration, ease);
	}

	#endregion
}

}
