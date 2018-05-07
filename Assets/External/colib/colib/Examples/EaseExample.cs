using System;
using System.Collections.Generic;
using UnityEngine;
using CoLib;

namespace CoLib.Example
{

public class EaseExample : MonoBehaviour
{

	void Awake()
	{
		_keys = new List<string>(_eases.Keys);
		_sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		_sphere.transform.parent = transform;
		_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_cube.transform.parent = transform;

		ResetEase();

	}

	void Update()
	{
		_queue.Update(Time.deltaTime);
	}


	void OnGUI()
	{
		Rect rect = new Rect(Screen.width / 2, 200, 100, 25);
		GUI.Label(rect, _keys[_position]); 
		rect.x += 150;
		rect.width = 70;
		if (GUI.Button(rect, "Next")) {
			_position += 1;
			_position %= _keys.Count;
			ResetEase();
		}

		rect.x -= 300;
		rect.width = 70;
		if (GUI.Button(rect, "Previous")) {
			_position -= 1;
			if (_position < 0) {
				_position = _keys.Count - 1;
			}
			ResetEase();
		}

	}

	void ResetEase()
	{
		Vector3 endPosition = Camera.main.ScreenToWorldPoint(
			new Vector3(Screen.width / 2 + 200, Screen.height / 2, 5)
		);

		Vector3 startPosition = Camera.main.ScreenToWorldPoint(
			new Vector3(Screen.width / 2 - 200, Screen.height / 2, 5)
		);

		Vector3 cubePosition = Camera.main.ScreenToWorldPoint(
			new Vector3(Screen.width / 2, Screen.height / 2 - 150, 5)
		);

		Quaternion startRotation = Quaternion.Euler(0f, 0f, 0f);
		Quaternion endRotation = Quaternion.Euler(0f, 0f, -180f);

		_sphere.transform.position = startPosition;
		_cube.transform.position = cubePosition;

		_queue = new CommandQueue();

		_queue.Enqueue(
			Commands.RepeatForever(
				Commands.Do( () => _sphere.transform.position = startPosition),
				Commands.Do( () => _cube.transform.rotation = startRotation),
				Commands.Parallel(
					Commands.MoveTo(_sphere, endPosition, 2f, _eases[_keys[_position]]),
					Commands.RotateBy(_cube, endRotation, 2f, _eases[_keys[_position]])
				),
				Commands.WaitForSeconds(0.5f)
			)
		 );
	}

	private int _position = 0;

	private GameObject _sphere;
	private GameObject _cube;
	private CommandQueue _queue = new CommandQueue();
		   
	private List<string> _keys;
	private Dictionary<string, CommandEase> _eases = new Dictionary<string, CommandEase>{
		{"Linear" , Ease.Linear()}, 
		{"InQuad", Ease.InQuad() }, {"OutQuad", Ease.OutQuad() }, {"InOutQuad", Ease.InOutQuad() },
		{"InCubic",Ease.InCubic()}, {"OutCubic",Ease.OutCubic()}, {"InOutCubic",Ease.InOutCubic()},
		{"InQuart",Ease.InQuart()}, {"OutQuart",Ease.OutQuart()}, {"InOutQuart",Ease.InOutQuart()},
		{"InQuint",Ease.InQuint()}, {"OutQuint",Ease.OutQuint()}, {"InOutQuint",Ease.InOutQuint()},
		{"InSin",Ease.InSin() }, {"OutSin",Ease.OutSin()}, {"InOutSin" ,Ease.InOutSin()}, 
		{"InElastic", Ease.InElastic()},  {"OutElastic", Ease.OutElastic()}, {"InOutElastic", Ease.InOutElastic()},
		{"InExpo",Ease.InExpo() }, {"OutExpo",Ease.OutExpo()}, {"InOutExpo",Ease.InOutExpo()}, 
		{"InCirc",Ease.InCirc() }, {"OutCirc",Ease.OutCirc()}, {"InOutCirc",Ease.InOutCirc()}, 
		{"InBack",Ease.InBack(0.5f) }, {"OutBack",Ease.OutBack(0.5f)},  {"InOutBack",Ease.InOutBack(0.5f)}, 
		{"InBounce",Ease.InBounce()}, {"OutBounce",Ease.OutBounce()}, {"InOutBounce",Ease.InOutBounce()}, 
		{"InHermite",Ease.InHermite()}, {"OutHermite",Ease.OutHermite()}, {"InOutHermite",Ease.InOutHermite()}, 
		{"RoundStep" ,Ease.RoundStep(4) },  { "CeilStep", Ease.CeilStep(4) }, {"FloorStep", Ease.FloorStep(4) }
	};
	
}

}


