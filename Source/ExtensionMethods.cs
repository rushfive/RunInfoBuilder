using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
    internal static class ListExtensionMethods
    {
		internal static int IndexOfFirstNull<T>(this List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == null) return i;
			}

			return -1;
		}
    }
}
