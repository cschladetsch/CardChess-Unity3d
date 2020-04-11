namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;

	/// <summary>
	/// Dialog.
	/// </summary>
	public class Dialog : MonoBehaviour, ITemplatable, IStylable
	{
		/// <summary>
		/// Class for the buttons instances.
		/// </summary>
		protected class ButtonsPool
		{
			/// <summary>
			/// Buttons templates.
			/// </summary>
			protected ReadOnlyCollection<Button> Templates;

			/// <summary>
			/// Active buttons.
			/// </summary>
			protected List<List<Button>> Active;

			/// <summary>
			/// Cached buttons.
			/// </summary>
			protected List<List<Button>> Cache;

			/// <summary>
			/// Labels for the buttons.
			/// </summary>
			protected List<List<string>> Labels;

			/// <summary>
			/// Actions on button click.
			/// </summary>
			protected List<List<UnityAction>> Actions;

			/// <summary>
			/// Count.
			/// </summary>
			public int Count
			{
				get
				{
					return Templates.Count;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ButtonsPool"/> class.
			/// </summary>
			/// <param name="templates">Templates.</param>
			/// <param name="active">List for the active buttons.</param>
			/// <param name="cache">List for the cached buttons.</param>
			/// <param name="labels">List for the buttons labels.</param>
			/// <param name="actions">List for the buttons actions.</param>
			public ButtonsPool(ReadOnlyCollection<Button> templates, List<List<Button>> active, List<List<Button>> cache, List<List<string>> labels, List<List<UnityAction>> actions)
			{
				Templates = templates;
				Active = active;
				Cache = cache;
				Labels = labels;
				Actions = actions;

				SetSize();
			}

			/// <summary>
			/// Set size.
			/// </summary>
			public void SetSize()
			{
				for (int i = 0; i < Templates.Count; i++)
				{
					Templates[i].gameObject.SetActive(false);
				}

				EnsureListSize(Active, Templates.Count);
				EnsureListSize(Cache, Templates.Count);
				EnsureListSize(Labels, Templates.Count);
				EnsureListSize(Actions, Templates.Count);
			}

			/// <summary>
			/// Ensure list size.
			/// </summary>
			/// <typeparam name="T">Type of the list data.</typeparam>
			/// <param name="list">List.</param>
			/// <param name="size">Required size.</param>
			protected static void EnsureListSize<T>(List<List<T>> list, int size)
			{
				for (int i = list.Count; i < size; i++)
				{
					list.Add(new List<T>());
				}

				for (int i = size; i > list.Count; i--)
				{
					list.RemoveAt(i - 1);
				}
			}

			/// <summary>
			/// Get the button.
			/// </summary>
			/// <param name="templateIndex">Index of the button template.</param>
			/// <param name="label">Button label.</param>
			/// <param name="action">Action on button click.</param>
			/// <returns>Button.</returns>
			public Button Get(int templateIndex, string label, UnityAction action)
			{
				var button = Instantiate(templateIndex, label, action);

				Actions[templateIndex].Add(action);
				Labels[templateIndex].Add(label);

				return button;
			}

			/// <summary>
			/// Create the button instance.
			/// </summary>
			/// <param name="templateIndex">Index of the button template.</param>
			/// <param name="label">Button label.</param>
			/// <param name="action">Action on button click.</param>
			/// <returns>Button.</returns>
			protected Button Instantiate(int templateIndex, string label, UnityAction action)
			{
				Button button;
				if (Cache[templateIndex].Count > 0)
				{
					button = Cache[templateIndex].Pop();
				}
				else
				{
					var template = Templates[templateIndex];
					button = Compatibility.Instantiate(template);
					button.transform.SetParent(template.transform.parent, false);
				}

				button.onClick.AddListener(action);

				button.gameObject.SetActive(true);
				button.transform.SetAsLastSibling();

				var dialog_button = button.GetComponentInChildren<DialogButtonComponentBase>();
				if (dialog_button != null)
				{
					dialog_button.SetButtonName(label);
				}
				else
				{
					var text = button.GetComponentInChildren<Text>();
					if (text != null)
					{
						text.text = label;
					}
				}

				Active[templateIndex].Add(button);

				return button;
			}

			/// <summary>
			/// Replace buttons templates.
			/// </summary>
			/// <param name="templates">Templates.</param>
			public void Replace(ReadOnlyCollection<Button> templates)
			{
				DisableButtons();
				ClearCache();

				Templates = templates;

				SetSize();

				for (int template_index = 0; template_index < Actions.Count; template_index++)
				{
					var labels = Labels[template_index];
					var actions = Actions[template_index];
					for (int i = 0; i < actions.Count; i++)
					{
						Instantiate(template_index, labels[i], actions[i]);
					}
				}
			}

			/// <summary>
			/// Disable.
			/// </summary>
			public void Disable()
			{
				DisableButtons();

				for (int template_index = 0; template_index < Active.Count; template_index++)
				{
					Labels[template_index].Clear();
					Actions[template_index].Clear();
				}
			}

			/// <summary>
			/// Disable buttons.
			/// </summary>
			protected void DisableButtons()
			{
				for (int template_index = 0; template_index < Active.Count; template_index++)
				{
					for (int i = 0; i < Active[template_index].Count; i++)
					{
						var button = Active[template_index][i];

						button.gameObject.SetActive(false);
						button.onClick.RemoveListener(Actions[template_index][i]);

						Cache[template_index].Add(button);
					}

					Active[template_index].Clear();
				}
			}

			/// <summary>
			/// Clear cache.
			/// </summary>
			protected void ClearCache()
			{
				for (int template_index = 0; template_index < Cache.Count; template_index++)
				{
					for (int i = 0; i < Cache[template_index].Count; i++)
					{
						Destroy(Cache[template_index][i]);
					}

					Cache[template_index].Clear();
				}
			}

			/// <summary>
			/// Execute action for each button.
			/// </summary>
			/// <param name="action">Action.</param>
			public void ForEach(Action<Button> action)
			{
				for (int i = 0; i < Templates.Count; i++)
				{
					action(Templates[i]);
					Cache[i].ForEach(action);
					Active[i].ForEach(action);
				}
			}
		}

		ButtonsPool buttonsPool;

		/// <summary>
		/// Buttons pool.
		/// </summary>
		protected ButtonsPool Buttons
		{
			get
			{
				if (buttonsPool == null)
				{
					buttonsPool = new ButtonsPool(ButtonsTemplates, ButtonsActive, ButtonsCached, ButtonsLabels, ButtonsActions);
				}

				return buttonsPool;
			}
		}

		/// <summary>
		/// The buttons in use.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<List<Button>> ButtonsActive = new List<List<Button>>();

		/// <summary>
		/// The cached buttons.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<List<Button>> ButtonsCached = new List<List<Button>>();

		/// <summary>
		/// The buttons labels.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<List<string>> ButtonsLabels = new List<List<string>>();

		/// <summary>
		/// The buttons actions.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<List<UnityAction>> ButtonsActions = new List<List<UnityAction>>();

#pragma warning disable 0649
		[SerializeField]
		[HideInInspector]
		[Obsolete("Replaced with buttonsTemplates")]
		Button defaultButton;
#pragma warning restore 0649

		/// <summary>
		/// Gets or sets the default button.
		/// </summary>
		/// <value>The default button.</value>
		[Obsolete("Replaced with ButtonsTemplates")]
		public Button DefaultButton
		{
			get
			{
				Upgrade();
				return defaultButton;
			}

			set
			{
				Upgrade();
				var buttons = new List<Button>(ButtonsTemplates)
				{
					value,
				};
				ButtonsTemplates = buttons.AsReadOnly();
			}
		}

		[SerializeField]
		List<Button> buttonsTemplates = new List<Button>();

		/// <summary>
		/// Gets or sets the default buttons.
		/// </summary>
		/// <value>The default buttons.</value>
		public ReadOnlyCollection<Button> ButtonsTemplates
		{
			get
			{
				Upgrade();

				return buttonsTemplates.AsReadOnly();
			}

			set
			{
				if (isTemplate && (buttonsTemplates.Count > value.Count))
				{
					throw new ArgumentOutOfRangeException("value", string.Format("Buttons count cannot be decreased. Current is {0}; New is {1}", buttonsTemplates.Count, value.Count));
				}

				Buttons.Replace(value);

				buttonsTemplates.Clear();
				buttonsTemplates.AddRange(value);
			}
		}

		/// <summary>
		/// Content root.
		/// </summary>
		[SerializeField]
		public RectTransform ContentRoot;

		[SerializeField]
		Text titleText;

		/// <summary>
		/// Gets or sets the text component.
		/// </summary>
		/// <value>The text.</value>
		public Text TitleText
		{
			get
			{
				return titleText;
			}

			set
			{
				titleText = value;
			}
		}

		[SerializeField]
		Text contentText;

		/// <summary>
		/// Gets or sets the text component.
		/// </summary>
		/// <value>The text.</value>
		public Text ContentText
		{
			get
			{
				return contentText;
			}

			set
			{
				contentText = value;
			}
		}

		[SerializeField]
		Image dialogIcon;

		/// <summary>
		/// Gets or sets the icon component.
		/// </summary>
		/// <value>The icon.</value>
		public Image Icon
		{
			get
			{
				return dialogIcon;
			}

			set
			{
				dialogIcon = value;
			}
		}

		DialogInfoBase dialogInfo;

		/// <summary>
		/// Gets the dialog info.
		/// </summary>
		/// <value>The dialog info.</value>
		public DialogInfoBase DialogInfo
		{
			get
			{
				if (dialogInfo == null)
				{
					dialogInfo = GetComponent<DialogInfoBase>();
				}

				return dialogInfo;
			}
		}

		bool isTemplate = true;

		/// <summary>
		/// Gets a value indicating whether this instance is template.
		/// </summary>
		/// <value><c>true</c> if this instance is template; otherwise, <c>false</c>.</value>
		public bool IsTemplate
		{
			get
			{
				return isTemplate;
			}

			set
			{
				isTemplate = value;
			}
		}

		/// <summary>
		/// Gets the name of the template.
		/// </summary>
		/// <value>The name of the template.</value>
		public string TemplateName
		{
			get;
			set;
		}

		static Templates<Dialog> templates;

		/// <summary>
		/// Dialog templates.
		/// </summary>
		public static Templates<Dialog> Templates
		{
			get
			{
				if (templates == null)
				{
					templates = new Templates<Dialog>();
				}

				return templates;
			}

			set
			{
				templates = value;
			}
		}

		/// <summary>
		/// Opened dialogs.
		/// </summary>
		protected static HashSet<Dialog> openedDialogs = new HashSet<Dialog>();

		/// <summary>
		/// List of the opened dialogs.
		/// </summary>
		protected static List<Dialog> OpenedDialogsList = new List<Dialog>();

		/// <summary>
		/// Opened dialogs.
		/// </summary>
		public static ReadOnlyCollection<Dialog> OpenedDialogs
		{
			get
			{
				OpenedDialogsList.Clear();
				OpenedDialogsList.AddRange(openedDialogs);

				return OpenedDialogsList.AsReadOnly();
			}
		}

		/// <summary>
		/// Count of the opened dialogs.
		/// </summary>
		public static int Opened
		{
			get
			{
				return openedDialogs.Count;
			}
		}

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		protected virtual void Awake()
		{
			if (IsTemplate)
			{
				gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Process enable event.
		/// </summary>
		protected virtual void OnEnable()
		{
			openedDialogs.Add(this);
		}

		/// <summary>
		/// Process disable event.
		/// </summary>
		protected virtual void OnDisable()
		{
			openedDialogs.Remove(this);
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			Buttons.Disable();

			if (!IsTemplate)
			{
				templates = null;
				return;
			}

			// if FindTemplates never called than TemplateName == null
			if (TemplateName != null)
			{
				Templates.Delete(TemplateName);
			}
		}

		/// <summary>
		/// Return dialog instance by the specified template name.
		/// </summary>
		/// <param name="templateName">Template name.</param>
		/// <returns>New Dialog instance.</returns>
		[Obsolete("Use Clone(templateName) instead.")]
		public static Dialog Template(string templateName)
		{
			return Clone(templateName);
		}

		/// <summary>
		/// Return dialog instance using current instance as template.
		/// </summary>
		/// <returns>New Dialog instance.</returns>
		[Obsolete("Use Clone() instead.")]
		public Dialog Template()
		{
			return Clone();
		}

		/// <summary>
		/// Return dialog instance by the specified template name.
		/// </summary>
		/// <param name="templateName">Template name.</param>
		/// <returns>New Dialog instance.</returns>
		public static Dialog Clone(string templateName)
		{
			return Templates.Instance(templateName);
		}

		/// <summary>
		/// Return dialog instance using current instance as template.
		/// </summary>
		/// <returns>New Dialog instance.</returns>
		public Dialog Clone()
		{
			if ((TemplateName != null) && Templates.Exists(TemplateName))
			{
				// do nothing
			}
			else if (!Templates.Exists(gameObject.name))
			{
				Templates.Add(gameObject.name, this);
			}
			else if (Templates.Get(gameObject.name) != this)
			{
				Templates.Add(gameObject.name, this);
			}

			var id = gameObject.GetInstanceID().ToString();
			if (!Templates.Exists(id))
			{
				Templates.Add(id, this);
			}
			else if (Templates.Get(id) != this)
			{
				Templates.Add(id, this);
			}

			return Templates.Instance(id);
		}

		/// <summary>
		/// The modal key.
		/// </summary>
		protected int? ModalKey;

		/// <summary>
		/// Callback on dialog close.
		/// </summary>
		protected Action OnClose;

		/// <summary>
		/// Callback on dialog cancel.
		/// </summary>
		protected Func<int, bool> OnCancel;

		/// <summary>
		/// Show dialog.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="buttons">Buttons.</param>
		/// <param name="focusButton">Set focus on button with specified name.</param>
		/// <param name="position">Position.</param>
		/// <param name="icon">Icon.</param>
		/// <param name="modal">If set to <c>true</c> modal.</param>
		/// <param name="modalSprite">Modal sprite.</param>
		/// <param name="modalColor">Modal color.</param>
		/// <param name="canvas">Canvas.</param>
		/// <param name="content">Content.</param>
		/// <param name="onClose">On close callback.</param>
		/// <param name="onCancel">On cancel callback.</param>
		public virtual void Show(
			string title = null,
			string message = null,
			IList<DialogButton> buttons = null,
			string focusButton = null,
			Vector3? position = null,
			Sprite icon = null,
			bool modal = false,
			Sprite modalSprite = null,
			Color? modalColor = null,
			Canvas canvas = null,
			RectTransform content = null,
			Action onClose = null,
			Func<int, bool> onCancel = null)
		{
			if (IsTemplate)
			{
				Debug.LogWarning("Use the template clone, not the template itself: DialogTemplate.Clone().Show(...), not DialogTemplate.Show(...)");
			}

			if (position == null)
			{
				position = new Vector3(0, 0, 0);
			}

			OnClose = onClose;
			OnCancel = onCancel;
			SetInfo(title, message, icon);
			SetContent(content);

			var parent = (canvas != null) ? canvas.transform : Utilites.FindTopmostCanvas(gameObject.transform);
			if (parent != null)
			{
				transform.SetParent(parent, false);
			}

			if (modal)
			{
				ModalKey = ModalHelper.Open(this, modalSprite, modalColor);
			}
			else
			{
				ModalKey = null;
			}

			transform.SetAsLastSibling();

			transform.localPosition = (Vector3)position;
			gameObject.SetActive(true);

			CreateButtons(buttons, focusButton);
		}

		/// <summary>
		/// Sets the info.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="icon">Icon.</param>
		public virtual void SetInfo(string title = null, string message = null, Sprite icon = null)
		{
			if (DialogInfo != null)
			{
				DialogInfo.SetInfo(title, message, icon);
			}
			else
			{
				if ((title != null) && (TitleText != null))
				{
					TitleText.text = title;
				}

				if ((message != null) && (ContentText != null))
				{
					ContentText.text = message;
				}

				if ((icon != null) && (Icon != null))
				{
					Icon.sprite = icon;
				}
			}
		}

		/// <summary>
		/// Sets the content.
		/// </summary>
		/// <param name="content">Content.</param>
		public virtual void SetContent(RectTransform content)
		{
			if (content == null)
			{
				return;
			}

			if (DialogInfo != null)
			{
				DialogInfo.SetContent(content);
			}
			else
			{
				if (ContentRoot != null)
				{
					content.SetParent(ContentRoot, false);
				}
			}
		}

		/// <summary>
		/// Cancel dialog.
		/// </summary>
		public virtual void Cancel()
		{
			if (OnCancel != null)
			{
				if (!OnCancel(-1))
				{
					return;
				}
			}

			Hide();
		}

		/// <summary>
		/// Close dialog.
		/// </summary>
		public virtual void Hide()
		{
			if (OnClose != null)
			{
				OnClose();
			}

			if (ModalKey != null)
			{
				ModalHelper.Close((int)ModalKey);
			}

			Return();
		}

		/// <summary>
		/// Creates the buttons.
		/// </summary>
		/// <param name="buttons">Buttons.</param>
		/// <param name="focusButton">Focus button.</param>
		protected virtual void CreateButtons(IList<DialogButton> buttons, string focusButton)
		{
			if (buttons == null)
			{
				return;
			}

			for (int index = 0; index < buttons.Count; index++)
			{
				var button = buttons[index];

				UnityAction callback = () =>
				{
					if (button.Action(index))
					{
						Hide();
					}
				};

				var template = GetTemplateIndex(button);

				var btn = Buttons.Get(template, button.Label, callback);

				if (button.Label == focusButton)
				{
					btn.Select();
				}
			}
		}

		/// <summary>
		/// Get button template index.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <returns>Template index,</returns>
		protected int GetTemplateIndex(DialogButton button)
		{
			var template = button.TemplateIndex;
			if (template < 0)
			{
				Debug.LogWarning(string.Format("Negative button index not supported. Button: {0}. Index: {1}.", button.Label, template));
				template = 0;
			}

			if (template >= Buttons.Count)
			{
				Debug.LogWarning(string.Format(
					"Not found button template with index {0} for the button: {1}. Available indices: 0..{2}",
					template,
					button.Label,
					Buttons.Count - 1));
				template = 0;
			}

			return template;
		}

		/// <summary>
		/// Return this instance to cache.
		/// </summary>
		protected virtual void Return()
		{
			Templates.ToCache(this);

			Buttons.Disable();
			ResetParametres();
		}

		/// <summary>
		/// Resets the parameters.
		/// </summary>
		protected virtual void ResetParametres()
		{
			var template = Templates.Get(TemplateName);

			OnClose = null;
			OnCancel = null;

			var title = template.TitleText != null ? template.TitleText.text : string.Empty;
			var content = template.ContentText != null ? template.ContentText.text : string.Empty;
			var icon = template.Icon != null ? template.Icon.sprite : null;

			SetInfo(title, content, icon);
		}

		/// <summary>
		/// Default function to close dialog.
		/// </summary>
		/// <returns>true if dialog can be closed; otherwise false.</returns>
		public static bool Close()
		{
			return true;
		}

		/// <summary>
		/// Default function to close dialog.
		/// </summary>
		/// <param name="index">Button index.</param>
		/// <returns>true if dialog can be closed; otherwise false.</returns>
		public static bool AlwaysClose(int index)
		{
			return true;
		}

		#region IStylable implementation

		/// <summary>
		/// Set the specified style.
		/// </summary>
		/// <returns><c>true</c>, if style was set for children gameobjects, <c>false</c> otherwise.</returns>
		/// <param name="style">Style data.</param>
		public bool SetStyle(Style style)
		{
			style.Dialog.Background.ApplyTo(GetComponent<Image>());
			style.Dialog.Title.ApplyTo(TitleText);

			style.Dialog.ContentBackground.ApplyTo(transform.Find("Content"));
			style.Dialog.ContentText.ApplyTo(ContentText);

			style.Dialog.Delimiter.ApplyTo(transform.Find("Delimiter/Delimiter"));

			style.ButtonClose.Background.ApplyTo(transform.Find("Header/CloseButton"));

			Buttons.ForEach(x => style.Dialog.Button.ApplyTo(x.gameObject));

			return true;
		}
		#endregion

		/// <summary>
		/// Upgrade fields data to new version.
		/// </summary>
		protected virtual void Upgrade()
		{
#pragma warning disable 0618
			if ((buttonsTemplates.Count == 0) && (defaultButton != null))
			{
				buttonsTemplates.Add(defaultButton);
			}
#pragma warning restore 0618
		}

#if UNITY_EDITOR
		/// <summary>
		/// Update layout when parameters changed.
		/// </summary>
		protected virtual void OnValidate()
		{
			Upgrade();

			if (ContentRoot == null)
			{
				ContentRoot = transform.Find("Content") as RectTransform;
			}
		}
#endif
	}
}