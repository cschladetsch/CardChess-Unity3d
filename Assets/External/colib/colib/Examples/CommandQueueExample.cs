using UnityEngine;
using System.Collections;
using CoLib;

namespace CoLib.Example
{

public class CommandQueueExample : MonoBehaviour 
{
	// Use this for initialization
	private void Start () 
	{
		bool condition = false;

		gameObject.Queue(
			Commands.ScaleTo(gameObject, 0.05f, 2.0f, Ease.OutQuart()),
			Commands.ScaleTo(gameObject, 1.0f, 1.0f, Ease.OutBounce()),
			Commands.RepeatForever(
				Commands.Parallel(
					Commands.Repeat(2,
						Commands.ScaleBy(gameObject, 1.5f, 1.0f, Ease.OutBounce())
					),
					Commands.RotateBy(gameObject, Quaternion.Euler(180.0f,0.0f, 90.0f), 0.25f, Ease.InOutHermite())
				),
				Commands.WaitForSeconds(0.25f),
				Commands.TintTo(gameObject, Color.red, 0.5f, Ease.InBack(0.2f)),
				Commands.TintBy(gameObject, Color.blue, 0.5f),
				Commands.Condition(delegate() { condition = !condition; return condition; }, 
					Commands.MoveBy(gameObject, new Vector3(0.0f, 2.0f, 0.0f), 0.25f, Ease.InOutHermite()),
					Commands.MoveBy(gameObject, new Vector3(0.0f, -2.0f, 0.0f), 0.25f, Ease.InOutHermite())
				),
				Commands.MoveTo(gameObject, new Vector3(0.0f, 0.0f, 0.0f), 0.25f, Ease.InOutHermite()),
				Commands.Parallel(
					Commands.ScaleTo(gameObject, 0.5f, 1.0f, Ease.OutBounce()),
					Commands.RotateTo(gameObject, Quaternion.identity, 0.5f, Ease.InOutHermite())
				),
				Commands.TintTo(gameObject, Color.white, 0.25f, Ease.InOutSin()),
				Commands.MoveFrom(gameObject,  new Vector3(0.0f, 0.0f, 0.8f), 0.5f, Ease.OutElastic()),
				Commands.RotateFrom(gameObject, Quaternion.Euler(0.0f, 45.0f, 45.0f), 0.5f, Ease.InOutExpo()),
				Commands.ScaleFrom(gameObject, 0.25f, 0.75f, Ease.InOutHermite()),
				Commands.TintFrom(gameObject, Color.green, 0.25f, Ease.InOutQuint()),
				Commands.ScaleTo(gameObject, 1.0f, 0.2f, Ease.Smooth()),
				Commands.Wiggle(gameObject.ToRotationRef(true), 30f, 2.0),
				Commands.Wobble(transform.ToPositionYRef(true), 3f, 2.0),
				Commands.Parallel(
					Commands.SquashAndStretch(gameObject.ToScaleRef(), 3f, 2.0),
					Commands.Shake(transform.ToPositionRef(true), 0.3f, 2.0),
					Commands.Shake(transform.ToRotationRef(true), 5f, 2.0)
				)
			)
		);		
	}
}

}
