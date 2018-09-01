using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal static class OptionTokenizer
	{
		internal static bool IsValidConfiguration(string input)
		{
			// todo: regex match
			return true;
		}

		// assumes input is valid, shoudl be validated before here!
		internal static (string FullKey, char? ShortKey) TokenizeKeyConfiguration(string input)
		{
			if (input.Contains("|"))
			{
				string[] split = input.Split('|');
				return (split[0].Trim(), split[1].Trim().ToCharArray().Single());
			}

			return (input.Trim(), null);
		}

		internal static (OptionType Type, string FullKey, List<char> ShortKeys, string Value) TokenizeProgramArgument(string argument)
		{
			if (!argument.StartsWith("--") && !argument.StartsWith("-"))
			{
				throw new ArgumentException("Options must begin with '--' or '-'.", nameof(argument));
			}

			char[] chars = argument.ToCharArray();
			if (chars.Count(c => c == '=') > 1 || chars.Last() == '=')
			{
				throw new ArgumentException("Options can only contain a single '=' and must not be the last character.", nameof(argument));
			}

			int invalidEqualsIndex = argument.StartsWith("--") ? 2 : 1;
			if (argument[invalidEqualsIndex] == '=')
			{
				throw new ArgumentException("Option keys cannot begin with '='.");
			}

			string value = null;

			var keyValueSplit = argument.Split('=');
			if (keyValueSplit.Length == 2)
			{
				value = keyValueSplit[1];
			}

			string key = keyValueSplit[0];

			if (key.StartsWith("--"))
			{
				return (OptionType.Full, key.Substring(2), null, value);
			}

			if (key.Length == 2)
			{
				return (OptionType.Short, null, new List<char> { key[1] }, value);
			}

			List<char> stackedKeys = key.Skip(1).ToList();
			if (stackedKeys.Count != stackedKeys.Distinct().Count())
			{
				throw new ArgumentException($"Stacked key token '{key}' contains duplicates.");
			}

			return (OptionType.Stacked, null, stackedKeys, value);
		}
	}
}
