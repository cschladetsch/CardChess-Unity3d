namespace UIWidgets
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Text proxy interface.
	/// </summary>
	public interface ITextProxy
	{
		/// <summary>
		/// Gameobject.
		/// </summary>
		GameObject GameObject
		{
			get;
		}

		/// <summary>
		/// Graphic component.
		/// </summary>
		Graphic Graphic
		{
			get;
		}

		/// <summary>
		/// Text.
		/// </summary>
		string Text
		{
			get;
			set;
		}
	}
}