using System;
using UnityEngine;

namespace CoLib
{

public  static class TransformExtensions 
{
	#region Extension methods

	public static Ref<Vector3> ToPositionRef(this Transform transform, bool isLocalSpace)
	{
		CheckTransformNonNull(transform);
		if (isLocalSpace) {
			return new Ref<Vector3>( 
				() => transform.localPosition,
				newPosition => transform.localPosition = newPosition
			);
		} else {
			return new Ref<Vector3>( 
				() => transform.position,
				newPosition => transform.position = newPosition
			);
		}
	}

	public static Ref<float> ToPositionXRef(this Transform transform, bool isLocalSpace)
	{
		CheckTransformNonNull(transform);

		if (isLocalSpace) {
			return new Ref<float>( 
				() => transform.localPosition.x,
				t => {
					var position = transform.localPosition;
					position.x = t;
					transform.localPosition = position;
				}
			);
		} else {
			return new Ref<float>( 
				() => transform.localPosition.x,
				t => {
					var position = transform.position;
					position.x = t;
					transform.position = position;
				}
			);
		}
	}

	public static Ref<float> ToPositionYRef(this Transform transform, bool isLocalSpace)
	{
		CheckTransformNonNull(transform);

		if (isLocalSpace) {
			return new Ref<float>( 
				() => transform.localPosition.y,
				t => {
					var position = transform.localPosition;
					position.y = t;
					transform.localPosition = position;
				}
			);
		} else {
			return new Ref<float>( 
				() => transform.localPosition.y,
				t => {
					var position = transform.position;
					position.y = t;
					transform.position = position;
				}
			);
		}
	}

	public static Ref<float> ToPositionZRef(this Transform transform, bool isLocalSpace)
	{
		CheckTransformNonNull(transform);

		if (isLocalSpace) {
			return new Ref<float>( 
				() => transform.localPosition.z,
				t => {
					var position = transform.localPosition;
					position.z = t;
					transform.localPosition = position;
				}
			);
		} else {
			return new Ref<float>( 
				() => transform.localPosition.z,
				t => {
					var position = transform.position;
					position.z = t;
					transform.position = position;
				}
			);
		}
	}

	public static Ref<Quaternion> ToRotationRef(this Transform transform, bool isLocalSpace)
	{
		CheckTransformNonNull(transform);

		if (isLocalSpace) {
			return new Ref<Quaternion>( 
				() => transform.localRotation,
				newRotation => transform.localRotation = newRotation
			);
		} else {
			return new Ref<Quaternion>( 
				() => transform.rotation,
				newRotation => transform.rotation = newRotation
			);
		}
	}

	public static Ref<float> ToRotationEulerXRef(this Transform transform, bool isLocalSpace)
	{
		CheckTransformNonNull(transform);

		if (isLocalSpace) {
			return new Ref<float>( 
				() => transform.localRotation.eulerAngles.x,
				newRotation => {
					var euler = transform.localRotation.eulerAngles;
					euler.x = newRotation;
					transform.localRotation = Quaternion.Euler(euler);
				}
			);
		} else {
			return new Ref<float>( 
				() => transform.rotation.eulerAngles.x,
				newRotation => {
					var euler = transform.rotation.eulerAngles;
					euler.x = newRotation;
					transform.rotation = Quaternion.Euler(euler);
				}
			);
		}
	}

	public static Ref<float> ToRotationEulerYRef(this Transform transform, bool isLocalSpace)
	{
		CheckTransformNonNull(transform);

		if (isLocalSpace) {
			return new Ref<float>( 
				() => transform.localRotation.eulerAngles.y,
				newRotation => {
					var euler = transform.localRotation.eulerAngles;
					euler.y = newRotation;
					transform.localRotation = Quaternion.Euler(euler);
				}
			);
		} else {
			return new Ref<float>( 
				() => transform.rotation.eulerAngles.y,
				newRotation => {
					var euler = transform.rotation.eulerAngles;
					euler.y = newRotation;
					transform.rotation = Quaternion.Euler(euler);
				}
			);
		}
	}

	public static Ref<float> ToRotationEulerZRef(this Transform transform, bool isLocalSpace)
	{
		CheckTransformNonNull(transform);

		if (isLocalSpace) {
			return new Ref<float>( 
				() => transform.localRotation.eulerAngles.z,
				newRotation => {
					var euler = transform.localRotation.eulerAngles;
					euler.z = newRotation;
					transform.localRotation = Quaternion.Euler(euler);
				}
			);
		} else {
			return new Ref<float>( 
				() => transform.rotation.eulerAngles.z,
				newRotation => {
					var euler = transform.rotation.eulerAngles;
					euler.z = newRotation;
					transform.rotation = Quaternion.Euler(euler);
				}
			);
		}
	}

	public static Ref<Vector3> ToScaleRef(this Transform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<Vector3>( 
			() => transform.localScale,
			newScale => transform.localScale = newScale
		);
	}

	public static Ref<float> ToScaleXRef(this Transform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<float>( 
			() => transform.localScale.x,
			t => {
				var scale = transform.localScale;
				scale.x = t;
				transform.localScale = scale;
			}
		);
	}

	public static Ref<float> ToScaleYRef(this Transform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<float>( 
			() => transform.localScale.y,
			t => {
				var scale = transform.localScale;
				scale.y = t;
				transform.localScale = scale;
			}
		);
	}

	public static Ref<float> ToScaleZRef(this Transform transform)
	{
		CheckTransformNonNull(transform);

		return new Ref<float>( 
			() => transform.localScale.z,
			t => {
				var scale = transform.localScale;
				scale.z = t;
				transform.localScale = scale;
			}
		);
	}

	#endregion

	#region Private methods

	private static void CheckTransformNonNull(Transform transform)
	{
		if (transform == null) {
			throw new ArgumentNullException("transform"); 
		}
	}

	#endregion
}

}
