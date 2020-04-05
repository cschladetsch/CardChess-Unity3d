namespace UIWidgets.Examples
{
	using System;
	using System.Collections.Generic;
	using UIWidgets;

	/// <summary>
	/// Layout with compact order and items at any line grouped by name.
	/// </summary>
	public class TrackLayoutByName : TrackLayoutAnyLineCompact<TrackData, DateTime>
	{
		/// <summary>
		/// Can be item placed along with the specified items.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="items">Items.</param>
		/// <returns>true if items can be places together; otherwise false.</returns>
		protected override bool CanBeWithItems(TrackData item, List<TrackData> items)
		{
			var valid_name = (items.Count == 0) ? true : (items[0].Name == item.Name);

			return valid_name && !IsIntersect(items, item.StartPoint, item.EndPoint);
		}
	}
}