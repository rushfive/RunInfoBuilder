using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace R5.RunInfoBuilder.Store
{
	internal interface IArgumentTypeResolver
	{
		ProgramArgumentType GetArgumentType(string argumentToken);
	}

	internal class ArgumentTypeResolver<TRunInfo> : IArgumentTypeResolver
		where TRunInfo : class
	{
		internal const string OptionRegex = @"^-{1,2}\b[A-Za-z0-9]+$";
		internal const string ArgumentRegex = @"^[A-Za-z0-9]+\b=.*$";
		
		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }
		private IArgumentTokenizer _tokenizer { get; }
		private IHelpManager<TRunInfo> _helpManager { get; }
		private IVersionManager _versionManager { get; }

		public ArgumentTypeResolver(
			IArgumentMetadata<TRunInfo> argumentMaps,
			IArgumentTokenizer tokenizer,
			IHelpManager<TRunInfo> helpManager,
			IVersionManager versionManager)
		{
			_argumentMaps = argumentMaps;
			_tokenizer = tokenizer;
			_helpManager = helpManager;
			_versionManager = versionManager;
		}

		public ProgramArgumentType GetArgumentType(string argumentToken)
		{
			if (string.IsNullOrWhiteSpace(argumentToken))
			{
				throw new ArgumentNullException(nameof(argumentToken), "Argument token must be provided.");
			}

			if (_argumentMaps.CommandExists(argumentToken))
			{
				return ProgramArgumentType.Command;
			}

			if (Regex.IsMatch(argumentToken, ArgumentTypeResolver<TRunInfo>.ArgumentRegex))
			{
				(string argumentKey, _) = _tokenizer.TokenizeArgument(argumentToken);

				bool validArgument = _argumentMaps.ArgumentExists(argumentKey);

				if (validArgument)
				{
					return ProgramArgumentType.Argument;
				}
			}

			if (Regex.IsMatch(argumentToken, ArgumentTypeResolver<TRunInfo>.OptionRegex))
			{
				(OptionType optionType, string fullKey, List<char> shortKeys) = _tokenizer.TokenizeOption(argumentToken);

				bool validOption = false;
				switch (optionType)
				{
					case OptionType.Full:
						validOption = _argumentMaps.FullOptionExists(fullKey);
						break;
					case OptionType.Short:
						validOption = _argumentMaps.ShortOptionExists(shortKeys.Single());
						break;
					case OptionType.ShortCompound:
						validOption = shortKeys.All(_argumentMaps.ShortOptionExists);
						break;
					default:
						throw new ArgumentOutOfRangeException($"'{optionType}' is not a valid option type.");
				}

				if (validOption)
				{
					return ProgramArgumentType.Option;
				}
			}

			if (_helpManager != null && _helpManager.IsTrigger(argumentToken))
			{
				return ProgramArgumentType.Help;
			}

			if (_versionManager != null && _versionManager.IsTrigger(argumentToken))
			{
				return ProgramArgumentType.Version;
			}

			return ProgramArgumentType.Unresolved;
		}
	}
}
