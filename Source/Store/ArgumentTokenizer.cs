using OLD.Validators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OLD.Store
{
	internal enum OptionType
	{
		Full,
		Short,
		ShortCompound
	}

	internal interface IArgumentTokenizer
	{
		(string key, string value) TokenizeArgument(string argumentToken);

		(OptionType type, string fullKey, List<char> shortKeys) TokenizeOption(string optionToken);
	}

	internal class ArgumentTokenizer : IArgumentTokenizer
	{
		public ArgumentTokenizer()
		{
		}

		public (string key, string value) TokenizeArgument(string argumentToken)
		{
			Debug.Assert(Regex.IsMatch(argumentToken, ArgumentTypeResolver<object>.ArgumentRegex));

			string[] split = argumentToken.Split('=');
			return (split[0], split[1]);
		}

		public (OptionType type, string fullKey, List<char> shortKeys) TokenizeOption(string optionToken)
		{
			Debug.Assert(Regex.IsMatch(optionToken, ArgumentTypeResolver<object>.OptionRegex));

			if (optionToken.StartsWith("--"))
			{
				return (OptionType.Full, optionToken.Substring(2), null);
			}

			if (optionToken.StartsWith("-"))
			{
				if (optionToken.Length == 2)
				{
					return (OptionType.Short, null, new List<char> { optionToken[1] });
				}
				if (optionToken.Length > 2)
				{
					return (OptionType.ShortCompound, null, optionToken.Skip(1).ToList());
				}
			}

			throw new ArgumentException($"Failed to tokenize option token '{optionToken}'.");
		}
	}
}
