namespace UIWidgets
{
	using System;

	/// <summary>
	/// Track data.
	/// </summary>
	/// <typeparam name="TPoint">Point type.</typeparam>
	public interface ITrackData<TPoint>
		where TPoint : IComparable
	{
		/// <summary>
		/// Start point.
		/// </summary>
		TPoint StartPoint
		{
			get;
		}

		/// <summary>
		/// End point.
		/// </summary>
		TPoint EndPoint
		{
			get;
		}

		/// <summary>
		/// Order.
		/// </summary>
		int Order
		{
			get;
			set;
		}

		/// <summary>
		/// Is order fixed?
		/// </summary>
		bool FixedOrder
		{
			get;
			set;
		}

		/// <summary>
		/// Is item dragged?
		/// </summary>
		bool IsDragged
		{
			get;
			set;
		}

		/// <summary>
		/// Move start point with end point to maintain same length.
		/// </summary>
		/// <param name="newStart">New start point.</param>
		void MoveStartPoint(TPoint newStart);

		/// <summary>
		/// Change StartPoint and EndPoint.
		/// </summary>
		/// <param name="newStart">New StartPoint.</param>
		/// <param name="newEnd">New EndPoint.</param>
		void ChangePoints(TPoint newStart, TPoint newEnd);
	}
}