using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal static class OptionHelper
	{
		// check if matches option regex (full | short?)
		public static bool IsValidProgramArgument(string argumentToken)
		{
			throw new NotImplementedException();
		}

		public static bool TryTokenize(string input, out (string fullKey, char? shortKey)? result)
		{
			throw new NotImplementedException();
		}

		public static bool IsValidOption(string argumentToken, (List<string>, List<char>) availableOptions)
		{
			// todo: implement
			// dont forget to check for compound/stacked options
			throw new NotImplementedException();
		}
	}
}
