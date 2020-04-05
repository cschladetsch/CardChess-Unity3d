namespace UIWidgets
{
	using System;
	using System.Linq;

	/// <summary>
	/// ListViewIcons.
	/// </summary>
	public class ListViewIcons : ListViewCustom<ListViewIconsItemComponent, ListViewIconsItemDescription>
	{
		static string GetItemName(ListViewIconsItemDescription item)
		{
			if (item == null)
			{
				return string.Empty;
			}

			return item.LocalizedName ?? item.Name;
		}

		[NonSerialized]
		bool isListViewIconsInited = false;

		/// <summary>
		/// Items comparison.
		/// </summary>
		protected readonly Comparison<ListViewIconsItemDescription> ItemsComparison =
			(x, y) => GetItemName(x).CompareTo(GetItemName(y));

		/// <summary>
		/// Init this instance.
		/// </summary>
		public override void Init()
		{
			if (isListViewIconsInited)
			{
				return;
			}

			isListViewIconsInited = true;

			base.Init();

			SortFunc = list => list.OrderBy<ListViewIconsItemDescription, string>(GetItemName);

			// DataSource.Comparison = ItemsComparison;
		}
	}
}