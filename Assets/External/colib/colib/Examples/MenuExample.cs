using UnityEngine;
using CoLib;
using System.Collections.Generic;


namespace CoLib.Example
{

public class MenuExample : MonoBehaviour
{
	private CommandQueue _queue = new CommandQueue();
	private List<MenuButton> _buttons = new List<MenuButton>();
	private MenuButton _backButton;

	const float RECT_WIDTH = 150f;
	const float RECT_HEIGHT = 50f;

	private class MenuButton
	{
		public Rect Region { get; set; }
		public float Rotation { get; set; }
		public string Text { get; set; }
		public System.Action OnClick { get; set; }

		public void Draw()
		{
			GUIUtility.RotateAroundPivot(Rotation, Region.center);
			if (GUI.Button(Region, Text)) {
				OnClick();
			}
			GUIUtility.RotateAroundPivot(-Rotation, Region.center);
		}

		public void Update(float deltaTime)
		{
			_queue.Update(Time.deltaTime);
			Vector3 mp = Input.mousePosition;
			mp.y = Screen.height - mp.y;
			if (Region.Contains(mp)) {
				AnimateHighlighted();
			} else {
				_highlighted = false;
			}
		}

		public Ref<float> ToScaleRef() 
		{
			return new Ref<float>(
				() => Region.width / RECT_WIDTH,
				(t) => {
					float newWidth = t * RECT_WIDTH;
					float newHeight = t * RECT_HEIGHT;
					Region = new Rect(
						Region.center.x - newWidth / 2f,
						Region.center.y - newHeight / 2f,
						newWidth, newHeight
					);
				}
			);
		}

		public Ref<float> ToRotationRef() 
		{
			return new Ref<float>(
				() => Rotation,
				(t) => Rotation = t
			);
		}

		public Ref<Vector2> ToPositionRef()
		{
			return new Ref<Vector2>(
				() => Region.center,
				(t) => {
					Region = new Rect(
						t.x - Region.width / 2f,
						t.y - Region.height/ 2f,
						Region.width, Region.height
					);
				}
			);
		}
	
		public void AnimateHighlighted()
		{
			if (_highlighted) {
				return;
			}
			_highlighted = true;
			_queue.Enqueue(
				Commands.While( () => _highlighted,
			    	Commands.Oscillate(ToScaleRef(), 0.1f, 0.5)
			    )
			);
		}

		private CommandQueue _queue = new CommandQueue();
		private bool _highlighted = false;
	}

	#region MonoBehaviour events

	void Awake()
	{
		Rect startRect = new Rect(
			(Screen.width - RECT_WIDTH) / 2,
			-RECT_HEIGHT,
			RECT_WIDTH,
			RECT_HEIGHT
		);


		_buttons.Add(new MenuButton { Region = startRect, Text = "Everything", OnClick = Example1 });
		_buttons.Add(new MenuButton { Region = startRect, Text = "Rectangles", OnClick = Example2 });
		_buttons.Add(new MenuButton { Region = startRect, Text = "Easing", OnClick = Example3 });
		//_buttons.Add(new MenuButton { Region = startRect, Text = "Example 4", OnClick = Example4 });

		_backButton = new MenuButton { Region = new Rect(-RECT_WIDTH,0f, RECT_WIDTH, RECT_HEIGHT), Text = "Back" };

		_queue.Enqueue(
			Commands.WaitForSeconds(1.0f)
		);
		AnimateIn();

	}

	void Update()
	{
		_queue.Update(Time.deltaTime);
		foreach (var button in _buttons) {
			button.Update(Time.deltaTime);
		}
		_backButton.Update(Time.deltaTime);
	}

	void OnGUI()
	{
		foreach (var button in _buttons) {
			button.Draw();
		}
		_backButton.Draw();
	}

	#endregion

	#region Animations

	private void AnimateIn()
	{
		List<CommandDelegate> _moves = new List<CommandDelegate>();

		Vector2 offset = new Vector2(
			Screen.width / 2f,
			Screen.height / 2f - RECT_HEIGHT * 1.5f * _buttons.Count / 2
		);

		float duration = 0.7f;
		float rotation = 360f;
		foreach (var button in _buttons) {
			_moves.Add(
				Commands.Parallel(
					Commands.ChangeBy( button.ToRotationRef(), rotation, duration, Ease.Smooth()),
					Commands.ChangeTo( button.ToPositionRef(), offset, duration, Ease.OutBounce())
				)
			);
			offset.y += RECT_HEIGHT * 1.5f;
			duration += 0.1f;
			rotation = - rotation;
		}
		_queue.Enqueue(
			Commands.ChangeTo(_backButton.ToPositionRef(), new Vector2(-RECT_WIDTH / 2, RECT_HEIGHT / 2), 0.3f, Ease.Smooth()), 
			Commands.Parallel( _moves.ToArray())
		);
	}

	private void AnimateOut()
	{
		if (_hidingMenu) {
			return;
		}
		_hidingMenu = true;
		Vector2 origin = new Vector2(Screen.width / 2f, -RECT_HEIGHT);

		List<CommandDelegate> _moves = new List<CommandDelegate>();
		foreach (var button in _buttons) {
			_moves.Add(Commands.ChangeTo(button.ToPositionRef(), origin, 0.7f, Ease.Smooth()));
		}

		_queue.Enqueue(
			Commands.Parallel(_moves.ToArray()),
			Commands.ChangeBy(_backButton.ToPositionRef(), new Vector2(RECT_WIDTH, 0f), 0.3f, Ease.Smooth()), 
			Commands.Do( () => _hidingMenu = false)
		);

	}

	#endregion

	#region Button Callbacks

	private void Example1() 
	{
		AnimateOut();
		GameObject gm = null;
		_queue.Enqueue(
			Commands.Do( () => {
				gm = GameObject.CreatePrimitive(PrimitiveType.Cube);
				gm.transform.position = Vector3.zero;
				gm.AddComponent<CommandQueueExample>();
			})
		);

		_backButton.OnClick = () => {
			Destroy(gm);
			AnimateIn();
		};
	}

	private void Example2()
	{
		AnimateOut();
		GameObject gm = null;
		_queue.Enqueue(
			Commands.Do( () => {
				gm = new GameObject();
				gm.AddComponent<RectangleExample>();
			})
		);
		_backButton.OnClick = () => {
			Destroy(gm);
			AnimateIn();
		};
	}

	private void Example3()
	{
		AnimateOut();

		GameObject gm = null;

		_queue.Enqueue(
			Commands.Do(() => {
				gm = new GameObject();

				gm.AddComponent<EaseExample>();
			})
		);

		_backButton.OnClick = () => {
			Destroy(gm);
			AnimateIn();
		};
	}

	private void Example4()
	{
		AnimateOut();
		_backButton.OnClick = () => {
			AnimateIn();
		};
	}

	#endregion

	#region Private fields

	private bool _hidingMenu = false;

	#endregion
}

}

