using R5.RunInfoBuilder.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace R5.RunInfoBuilder.Validators
{
	internal interface IArgumentTypeResolver
	{
		bool TryGetArgumentType(string argumentToken, out ProgramArgumentType type);
	}

	internal class ArgumentTypeResolver<TRunInfo> : IArgumentTypeResolver
		where TRunInfo : class
	{
		internal const string OptionRegex = @"^-{1,2}\b[A-Za-z0-9]+$";
		//internal const string ArgumentRegex = @"^[A-Za-z0-9]+\b=\b[\w/.-]+$";
		internal const string ArgumentRegex = @"^[A-Za-z0-9]+\b=.*$";

		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }
		private IArgumentTokenizer _tokenizer { get; }

		public ArgumentTypeResolver(
			IArgumentMetadata<TRunInfo> argumentMaps,
			IArgumentTokenizer tokenizer)
		{
			_argumentMaps = argumentMaps;
			_tokenizer = tokenizer;
		}

		public bool TryGetArgumentType(string argumentToken, out ProgramArgumentType type)
		{
			type = default;

			if (string.IsNullOrWhiteSpace(argumentToken))
			{
				return false;
			}

			if (_argumentMaps.CommandExists(argumentToken))
			{
				type = ProgramArgumentType.Command;
				return true;
			}

			if (Regex.IsMatch(argumentToken, ArgumentTypeResolver<TRunInfo>.ArgumentRegex))
			{
				type = ProgramArgumentType.Argument;

				(string argumentKey, _) = _tokenizer.TokenizeArgument(argumentToken);

				return _argumentMaps.ArgumentExists(argumentKey);
			}

			if (Regex.IsMatch(argumentToken, ArgumentTypeResolver<TRunInfo>.OptionRegex))
			{
				type = ProgramArgumentType.Option;

				(OptionType optionType, string fullKey, List<char> shortKeys) = _tokenizer.TokenizeOption(argumentToken);

				switch (optionType)
				{
					case OptionType.Full:
						return _argumentMaps.FullOptionExists(fullKey);
					case OptionType.Short:
						return _argumentMaps.ShortOptionExists(shortKeys.Single());
					case OptionType.ShortCompound:
						return shortKeys.All(_argumentMaps.ShortOptionExists);
					default:
						throw new ArgumentOutOfRangeException($"'{type}' is not a valid option type.");
				}
			}

			return false;
		}
	}
}
