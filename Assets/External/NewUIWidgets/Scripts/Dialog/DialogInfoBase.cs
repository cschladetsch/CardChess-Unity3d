namespace UIWidgets
{
	using UnityEngine;

	/// <summary>
	/// DialogInfoBase.
	/// </summary>
	public abstract class DialogInfoBase : MonoBehaviour
	{
		/// <summary>
		/// Content.
		/// </summary>
		[SerializeField]
		public RectTransform ContentRoot;

		/// <summary>
		/// Sets the info.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="icon">Icon.</param>
		public abstract void SetInfo(string title, string message, Sprite icon);

		/// <summary>
		/// Set content.
		/// </summary>
		/// <param name="content">Content.</param>
		public virtual void SetContent(RectTransform content)
		{
			if (content == null)
			{
				return;
			}

			if (ContentRoot == null)
			{
				Debug.LogWarning("ContentRoot not specified.", this);
				return;
			}

			content.SetParent(ContentRoot, false);
		}
	}
}