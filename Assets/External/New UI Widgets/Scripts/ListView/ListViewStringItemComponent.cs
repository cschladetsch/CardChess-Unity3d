namespace UIWidgets
{
	using UnityEngine.UI;

	/// <summary>
	/// ListViewString component.
	/// </summary>
	public class ListViewStringItemComponent : ListViewItem, IViewData<string>
	{
		/// <summary>
		/// The Text component.
		/// </summary>
		public TextAdapter Text;

		/// <summary>
		/// Item.
		/// </summary>
		public string Item
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets foreground graphics for coloring.
		/// </summary>
		public override Graphic[] GraphicsForeground
		{
			get
			{
				return new Graphic[] { Text.Graphic, };
			}
		}

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="item">Text.</param>
		public virtual void SetData(string item)
		{
			Item = item;
			Text.Value = item.Replace("\\n", "\n");
		}
	}
}