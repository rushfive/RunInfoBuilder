using R5.RunInfoBuilder.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Validators
{
	internal interface IValidationHelper
	{
		bool OptionClashesWithExisting(string fullKey, char? shortKey);

		bool IsPermutation(string token, string chars);
	}

	internal class ValidationHelper<TRunInfo> : IValidationHelper
		where TRunInfo : class
	{
		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }

		public ValidationHelper(IArgumentMetadata<TRunInfo> argumentMaps)
		{
			_argumentMaps = argumentMaps;
		}

		public bool OptionClashesWithExisting(string fullKey, char? shortKey)
		{
			// 1. get full set of chars (current short options + shortKey if exists)
			List<OptionMetadata> options = _argumentMaps.GetOptions();

			List<char> shortKeys = options
				.Where(o => o.ShortKey.HasValue)
				.Select(o => o.ShortKey.Value)
				.ToList();

			if (shortKey.HasValue)
			{
				shortKeys.Add(shortKey.Value);
			}

			List<string> fullKeys = options
				.Select(o => o.FullKey)
				.ToList();

			fullKeys.Add(fullKey);

			// 2. foreach existing full option key + current, check if permutation
			foreach(string key in fullKeys)
			{
				if (IsPermutation(key, new string(shortKeys.ToArray())))
				{
					return true;
				}
			}

			return false;
		}

		public bool IsPermutation(string token, string chars)
		{
			if (string.IsNullOrWhiteSpace(token))
			{
				throw new ArgumentNullException(nameof(token), "Token must be provided.");
			}
			if (string.IsNullOrWhiteSpace(chars))
			{
				throw new ArgumentNullException(nameof(chars), "Chars string must be provided.");
			}

			if (token.Length > chars.Length)
			{
				return false;
			}

			return IsPermutationInternal("", token, chars);
		}

		private bool IsPermutationInternal(string current, string token, string chars)
		{
			if (current.Length == token.Length)
			{
				return current == token;
			}

			for (int i = 0; i < chars.Length; i++)
			{
				string beg = chars.Substring(0, i);
				string end = i == chars.Length - 1
					? ""
					: chars.Substring(i + 1);

				bool isPermutation = IsPermutationInternal(current + chars[i], token, beg + end);
				if (isPermutation)
				{
					return true;
				}
			}

			return false;
		}
	}
}
