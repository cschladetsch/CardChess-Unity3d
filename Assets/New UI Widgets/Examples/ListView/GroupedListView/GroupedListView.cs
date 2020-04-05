namespace UIWidgets.Examples
{
	using UIWidgets;

	/// <summary>
	/// GroupedListView
	/// </summary>
	public class GroupedListView : ListViewCustomHeight<GroupedListViewComponent, IGroupedListItem>
	{
		/// <summary>
		/// Grouped data.
		/// </summary>
		public GroupedItems GroupedData = new GroupedItems();

		bool isGroupedListViewInited;

		/// <summary>
		/// Init this instance.
		/// </summary>
		public override void Init()
		{
			if (isGroupedListViewInited)
			{
				return;
			}

			isGroupedListViewInited = true;

			base.Init();

			GroupedData.GroupComparison = (x, y) => x.Name.CompareTo(y.Name);
			GroupedData.Data = DataSource;
		}

		/// <summary>
		/// Set highlights colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void HighlightColoring(GroupedListViewComponent component)
		{
			var is_group = (component != null) && (DataSource[component.Index] is GroupedListGroup);

			// skip highlight if group item
			if (is_group)
			{
				return;
			}

			base.HighlightColoring(component);
		}
	}
}