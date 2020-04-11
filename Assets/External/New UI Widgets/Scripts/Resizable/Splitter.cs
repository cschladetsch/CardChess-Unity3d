namespace UIWidgets
{
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.Serialization;
	using UnityEngine.UI;

	/// <summary>
	/// Splitter type.
	/// </summary>
	public enum SplitterType
	{
		/// <summary>
		/// Horizontal.
		/// </summary>
		Horizontal = 0,

		/// <summary>
		/// Vertical.
		/// </summary>
		Vertical = 1,
	}

	/// <summary>
	/// Splitter mode.
	/// </summary>
	public enum SplitterMode
	{
		/// <summary>
		/// Auto mode. Use previous and next siblings in hierarchy.
		/// </summary>
		Auto = 0,

		/// <summary>
		/// Manual mode. Use specified targets to resize.
		/// </summary>
		Manual = 1,
	}

	/// <summary>
	/// Splitter.
	/// </summary>
	[AddComponentMenu("UI/New UI Widgets/Splitter")]
	public class Splitter : MonoBehaviour,
		IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,
		IPointerEnterHandler, IPointerExitHandler
	{
		/// <summary>
		/// The type.
		/// </summary>
		public SplitterType Type = SplitterType.Vertical;

		/// <summary>
		/// Is need to update RectTransform on Resize.
		/// </summary>
		[SerializeField]
		public bool UpdateRectTransforms = true;

		/// <summary>
		/// Is need to update LayoutElement on Resize.
		/// </summary>
		[SerializeField]
		public bool UpdateLayoutElements = true;

		/// <summary>
		/// The current camera. For Screen Space - Overlay let it empty.
		/// </summary>
		[SerializeField]
		public Camera CurrentCamera;

		/// <summary>
		/// The cursor texture.
		/// </summary>
		[SerializeField]
		public Texture2D CursorTexture;

		/// <summary>
		/// The cursor hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 CursorHotSpot = new Vector2(16, 16);

		/// <summary>
		/// The default cursor texture.
		/// </summary>
		[SerializeField]
		public Texture2D DefaultCursorTexture;

		/// <summary>
		/// The default cursor hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 DefaultCursorHotSpot;

		/// <summary>
		/// Start resize event.
		/// </summary>
		public SplitterResizeEvent OnStartResize = new SplitterResizeEvent();

		/// <summary>
		/// During resize event.
		/// </summary>
		public SplitterResizeEvent OnResize = new SplitterResizeEvent();

		/// <summary>
		/// End resize event.
		/// </summary>
		public SplitterResizeEvent OnEndResize = new SplitterResizeEvent();

		RectTransform rectTransform;

		/// <summary>
		/// Gets the RectTransform.
		/// </summary>
		/// <value>The RectTransform.</value>
		public RectTransform RectTransform
		{
			get
			{
				if (rectTransform == null)
				{
					rectTransform = transform as RectTransform;
				}

				return rectTransform;
			}
		}

		Canvas canvas;

		/// <summary>
		/// Mode.
		/// </summary>
		[SerializeField]
		protected SplitterMode Mode = SplitterMode.Auto;

		/// <summary>
		/// Previous object.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("leftTarget")]
		protected RectTransform PreviousObject;

		/// <summary>
		/// Next object.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("rightTarget")]
		protected RectTransform NextObject;

		LayoutElement previousLayoutElement;

		/// <summary>
		/// LayoutElement of the previous target.
		/// </summary>
		protected LayoutElement PreviousLayoutElement
		{
			get
			{
				if ((previousLayoutElement == null) || (previousLayoutElement.gameObject != PreviousObject.gameObject))
				{
					previousLayoutElement = Utilites.GetOrAddComponent<LayoutElement>(PreviousObject);
				}

				return previousLayoutElement;
			}
		}

		LayoutElement nextLayoutElement;

		/// <summary>
		/// LayoutElement of the next target.
		/// </summary>
		protected LayoutElement NextLayoutElement
		{
			get
			{
				if ((nextLayoutElement == null) || (nextLayoutElement.gameObject != NextObject.gameObject))
				{
					nextLayoutElement = Utilites.GetOrAddComponent<LayoutElement>(NextObject);
				}

				return nextLayoutElement;
			}
		}

		Vector2 summarySize;

		bool processDrag;

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected virtual void Start()
		{
			Init();
		}

		/// <summary>
		/// Raises the initialize potential drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnInitializePotentialDrag(PointerEventData eventData)
		{
			Init();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
			canvas = Utilites.FindTopmostCanvas(transform).GetComponent<Canvas>();

			LayoutUtilites.UpdateLayout(transform.parent.GetComponent<LayoutGroup>());
			transform.parent.GetComponentsInChildren<Splitter>().ForEach(x => x.InitSizes());
		}

		/// <summary>
		/// Init sizes.
		/// </summary>
		protected void InitSizes()
		{
			var index = transform.GetSiblingIndex();

			if (index == 0 || transform.parent.childCount == (index + 1))
			{
				return;
			}

			if (Mode == SplitterMode.Auto)
			{
				PreviousObject = transform.parent.GetChild(index - 1) as RectTransform;
				NextObject = transform.parent.GetChild(index + 1) as RectTransform;
			}

			PreviousLayoutElement.preferredWidth = PreviousObject.rect.width;
			PreviousLayoutElement.preferredHeight = PreviousObject.rect.height;

			NextLayoutElement.preferredWidth = NextObject.rect.width;
			NextLayoutElement.preferredHeight = NextObject.rect.height;
		}

		bool cursorChanged = false;

		/// <summary>
		/// Is cursor over?
		/// </summary>
		protected bool IsCursorOver;

		/// <summary>
		/// Called by a BaseInputModule when an OnPointerEnter event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerEnter(PointerEventData eventData)
		{
			IsCursorOver = true;
		}

		/// <summary>
		/// Called by a BaseInputModule when an OnPointerExit event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerExit(PointerEventData eventData)
		{
			IsCursorOver = false;

			cursorChanged = false;
			Cursor.SetCursor(DefaultCursorTexture, DefaultCursorHotSpot, Compatibility.GetCursorMode());
		}

		/// <summary>
		/// Update cursor.
		/// </summary>
		protected virtual void LateUpdate()
		{
			if (!IsCursorOver)
			{
				return;
			}

			if (processDrag)
			{
				return;
			}

			if (CursorTexture == null)
			{
				return;
			}

			if (!Input.mousePresent)
			{
				return;
			}

			Vector2 point;

			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, Input.mousePosition, CurrentCamera, out point))
			{
				return;
			}

			var rect = RectTransform.rect;
			if (rect.Contains(point))
			{
				cursorChanged = true;
				Cursor.SetCursor(CursorTexture, CursorHotSpot, Compatibility.GetCursorMode());
			}
			else if (cursorChanged)
			{
				cursorChanged = false;
				Cursor.SetCursor(DefaultCursorTexture, DefaultCursorHotSpot, Compatibility.GetCursorMode());
			}
		}

		/// <summary>
		/// Raises the begin drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnBeginDrag(PointerEventData eventData)
		{
			Vector2 point;
			processDrag = false;

			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.pressPosition, eventData.pressEventCamera, out point))
			{
				return;
			}

			var index = transform.GetSiblingIndex();

			if (index == 0 || transform.parent.childCount == (index + 1))
			{
				return;
			}

			Cursor.SetCursor(CursorTexture, CursorHotSpot, Compatibility.GetCursorMode());
			cursorChanged = true;

			processDrag = true;

			if (Mode == SplitterMode.Auto)
			{
				PreviousObject = transform.parent.GetChild(index - 1) as RectTransform;
				NextObject = transform.parent.GetChild(index + 1) as RectTransform;
			}

			PreviousLayoutElement.preferredWidth = PreviousObject.rect.width;
			PreviousLayoutElement.preferredHeight = PreviousObject.rect.height;

			NextLayoutElement.preferredWidth = NextObject.rect.width;
			NextLayoutElement.preferredHeight = NextObject.rect.height;

			summarySize = new Vector2(PreviousObject.rect.width + NextObject.rect.width, PreviousObject.rect.height + NextObject.rect.height);

			OnStartResize.Invoke(this);
		}

		/// <summary>
		/// Raises the end drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnEndDrag(PointerEventData eventData)
		{
			Cursor.SetCursor(DefaultCursorTexture, DefaultCursorHotSpot, Compatibility.GetCursorMode());
			cursorChanged = false;

			if (processDrag)
			{
				processDrag = false;

				OnEndResize.Invoke(this);
			}
		}

		/// <summary>
		/// Raises the drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnDrag(PointerEventData eventData)
		{
			if (!processDrag)
			{
				return;
			}

			if (canvas == null)
			{
				throw new MissingComponentException(gameObject.name + " not in Canvas hierarchy.");
			}

			Vector2 p1;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, CurrentCamera, out p1);
			Vector2 p2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position - eventData.delta, CurrentCamera, out p2);
			var delta = p1 - p2;

			if (UpdateRectTransforms)
			{
				PerformUpdateRectTransforms(delta);
			}

			if (UpdateLayoutElements)
			{
				PerformUpdateLayoutElements(delta);
			}

			OnResize.Invoke(this);
		}

		/// <summary>
		/// Is horizontal direction?
		/// </summary>
		/// <returns>true if direction is horizontal; otherwise false.</returns>
		protected bool IsHorizontal()
		{
			return Type == SplitterType.Horizontal;
		}

		/// <summary>
		/// Update RectTransform sizes.
		/// </summary>
		/// <param name="delta">Size delta.</param>
		protected void PerformUpdateRectTransforms(Vector2 delta)
		{
			if (!IsHorizontal())
			{
				float left_width;
				float right_width;

				if (delta.x > 0)
				{
					left_width = Mathf.Min(PreviousLayoutElement.preferredWidth + delta.x, summarySize.x - NextLayoutElement.minWidth);
					right_width = summarySize.x - PreviousLayoutElement.preferredWidth;
				}
				else
				{
					right_width = Mathf.Min(NextLayoutElement.preferredWidth - delta.x, summarySize.x - PreviousLayoutElement.minWidth);
					left_width = summarySize.x - NextLayoutElement.preferredWidth;
				}

				PreviousObject.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, left_width);
				NextObject.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, right_width);
			}
			else
			{
				float left_height;
				float right_height;

				delta.y *= -1;
				if (delta.y > 0)
				{
					left_height = Mathf.Min(PreviousLayoutElement.preferredHeight + delta.y, summarySize.y - NextLayoutElement.minHeight);
					right_height = summarySize.y - PreviousLayoutElement.preferredHeight;
				}
				else
				{
					right_height = Mathf.Min(NextLayoutElement.preferredHeight - delta.y, summarySize.y - PreviousLayoutElement.minHeight);
					left_height = summarySize.y - NextLayoutElement.preferredHeight;
				}

				PreviousObject.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, left_height);
				NextObject.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, right_height);
			}
		}

		/// <summary>
		/// Update LayouElement sizes.
		/// </summary>
		/// <param name="delta">Size delta.</param>
		protected void PerformUpdateLayoutElements(Vector2 delta)
		{
			if (!IsHorizontal())
			{
				if (delta.x > 0)
				{
					PreviousLayoutElement.preferredWidth = Mathf.Min(PreviousLayoutElement.preferredWidth + delta.x, summarySize.x - NextLayoutElement.minWidth);
					NextLayoutElement.preferredWidth = summarySize.x - PreviousLayoutElement.preferredWidth;
				}
				else
				{
					NextLayoutElement.preferredWidth = Mathf.Min(NextLayoutElement.preferredWidth - delta.x, summarySize.x - PreviousLayoutElement.minWidth);
					PreviousLayoutElement.preferredWidth = summarySize.x - NextLayoutElement.preferredWidth;
				}
			}
			else
			{
				delta.y *= -1;
				if (delta.y > 0)
				{
					PreviousLayoutElement.preferredHeight = Mathf.Min(PreviousLayoutElement.preferredHeight + delta.y, summarySize.y - NextLayoutElement.minHeight);
					NextLayoutElement.preferredHeight = summarySize.y - PreviousLayoutElement.preferredHeight;
				}
				else
				{
					NextLayoutElement.preferredHeight = Mathf.Min(NextLayoutElement.preferredHeight - delta.y, summarySize.y - PreviousLayoutElement.minHeight);
					PreviousLayoutElement.preferredHeight = summarySize.y - NextLayoutElement.preferredHeight;
				}
			}
		}
	}
}