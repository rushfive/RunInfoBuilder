using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder
{
	internal static class ExtensionMethods
	{
		internal static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			if (items == null || !items.Any())
			{
				throw new ArgumentNullException(nameof(items), "Items to add must be provided.");
			}

			foreach (T item in items)
			{
				collection.Add(item);
			}

			return collection;
		}

		internal static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> applyFunction)
		{
			foreach (T item in enumerable)
			{
				applyFunction(item);
			}

			return enumerable;
		}

		internal static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null || !enumerable.Any())
			{
				return true;
			}
			return false;
		}
	}
}
