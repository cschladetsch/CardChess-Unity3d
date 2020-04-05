namespace UIWidgets
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// TextUnity.
	/// </summary>
	public class TextUnity : ITextProxy
	{
		/// <summary>
		/// Text component.
		/// </summary>
		protected Text Component;

		/// <summary>
		/// GameObject.
		/// </summary>
		public GameObject GameObject
		{
			get
			{
				return Component.gameObject;
			}
		}

		/// <summary>
		/// Text.
		/// </summary>
		public string Text
		{
			get
			{
				return Component.text;
			}

			set
			{
				Component.text = value;
			}
		}

		/// <summary>
		/// Graphic component.
		/// </summary>
		public Graphic Graphic
		{
			get
			{
				return Component;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextUnity"/> class.
		/// </summary>
		/// <param name="component">Component.</param>
		public TextUnity(Text component)
		{
			Component = component;
		}
	}
}