using System;
using System.Collections.Generic;
using System.Linq;
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

		public static (string FullKey, char? ShortKey) Tokenize(string input)
		{
			if (input.Contains("|"))
			{
				string[] split = input.Split('|');
				return (split[0].Trim(), split[1].Trim().ToCharArray().Single());
			}

			return (input.Trim(), null);
		}

		// TODO: Argument Sequence processing needs to be refactored, diff way to handle options
		public static bool IsValidOption(string argumentToken, (List<string>, List<char>) availableOptions)
		{
			// todo: implement
			// dont forget to check for compound/stacked options
			throw new NotImplementedException();
		}
	}
}
