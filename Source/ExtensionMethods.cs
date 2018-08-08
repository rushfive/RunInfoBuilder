using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
    public static class ListExtensionMethods
    {
		public static bool NullOrEmpty<T>(this List<T> list)
		{
			return list == null || list.Count == 0;
		}
    }
}
