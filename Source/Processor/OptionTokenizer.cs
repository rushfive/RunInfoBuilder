using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal interface IOptionTokenizer
	{
		// check if matches option regex (full | short?)
		bool IsValidProgramArgument(string argumentToken);

		bool TryTokenize(string input, out (string fullKey, char? shortKey)? result);
	}

	internal class OptionTokenizer : IOptionTokenizer
	{

		public OptionTokenizer()
		{

		}

		public bool IsValidProgramArgument(string argumentToken)
		{
			throw new NotImplementedException();
		}

		public bool TryTokenize(string input, out (string fullKey, char? shortKey)? result)
		{
			throw new NotImplementedException();
		}
	}
}
