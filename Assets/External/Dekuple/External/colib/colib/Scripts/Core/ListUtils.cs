using System;
using System.Collections.Generic;

namespace CoLib.Utils
{

public class FunctionalComparer<T> : IComparer<T>
{
	#region Public methods

	public FunctionalComparer(Func<T, T, int> compare)
	{
		if (compare == null)
		{
			throw new ArgumentNullException("comparison");
		}
		_comparison = new Comparison<T>(compare);
	}

	#endregion

	#region IComparer

	public int Compare(T x, T y)
	{
		return _comparison(x, y);
	}

	#endregion

	#region Private fields

	private readonly Comparison<T> _comparison;

	#endregion
}

public static class ListUtils 
{
	/// <summary>
	/// Searches a list using a binary search, and a comparision function.
	/// </summary>
	/// <returns>The index of the matched item, or -1 if no item matched.</returns>
	/// <param name="list">The list to search.</param>
	/// <param name="item">The item to compare to.</param>
	/// <param name="compare">The comparison function</param>
	public static int BinarySearch<T>(this List<T> list, T item, Func<T, T, int> compare)
	{
		return list.BinarySearch(item, new FunctionalComparer<T>(compare));
    }

	/// <summary>
	/// Searches a list using a binary search, and a comparision function.
	/// </summary>
	/// <returns>The index of the matched item, or -1 if no item matched.</returns>
	/// <param name="list">The list to search.</param>
	/// <param name="compare">The comparison function</param>
	public static int BinarySearch<T>(this List<T> list, Func<T, int> compare)
    	where T : class
	{
		Func<T, T, int> newCompare = (a, b) => compare(a);
		return list.BinarySearch((T)null, newCompare);
	}

	/// <summary>
	/// Searches a list using a binary search, and a comparision function.
	/// </summary>
	/// <returns>The index of the matched item, or -1 if no item matched.</returns>
	/// <param name="list">The list to search.</param>
	/// <param name="compare">The comparison function</param>
	public static int BinarySearchStruct<T>(this List<T> list, Func<T, int> compare)
    	where T : struct
	{
		T dummyT = new T();

		Func<T, T, int> newCompare = (a, b) => compare(b);
		return list.BinarySearch(dummyT, newCompare);
	}
}

}
