using System;
using System.Collections.Generic;
using System.Text;

namespace R5.Lib.Functional.Parse
{
	using static F;

    public static class Int
    {
		public static Option<int> Parse(string s)
		{
			return int.TryParse(s, out int result)
				? Some(result) : None;
		}
    }
}
