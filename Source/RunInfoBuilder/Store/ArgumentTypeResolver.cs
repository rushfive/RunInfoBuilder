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

		public bool TryGetArgumentType(string argumentToken, out ProgramArgumentType type)
		{
			type = ProgramArgumentType.Unresolved;

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
				(string argumentKey, _) = _tokenizer.TokenizeArgument(argumentToken);

				bool isValid = _argumentMaps.ArgumentExists(argumentKey);

				if (isValid)
				{
					type = ProgramArgumentType.Argument;
					return true;
				}

				return false;
			}

			if (Regex.IsMatch(argumentToken, ArgumentTypeResolver<TRunInfo>.OptionRegex))
			{
				(OptionType optionType, string fullKey, List<char> shortKeys) = _tokenizer.TokenizeOption(argumentToken);

				bool optionValid = false;
				switch (optionType)
				{
					case OptionType.Full:
						optionValid = _argumentMaps.FullOptionExists(fullKey);
						break;
					case OptionType.Short:
						optionValid = _argumentMaps.ShortOptionExists(shortKeys.Single());
						break;
					case OptionType.ShortCompound:
						optionValid = shortKeys.All(_argumentMaps.ShortOptionExists);
						break;
					default:
						throw new ArgumentOutOfRangeException($"'{type}' is not a valid option type.");
				}

				if (optionValid)
				{
					type = ProgramArgumentType.Option;
					return true;
				}

				return false;
			}

			if (_helpManager != null && _helpManager.IsTrigger(argumentToken))
			{
				type = ProgramArgumentType.Help;
				return true;
			}

			if (_versionManager != null && _versionManager.IsTrigger(argumentToken))
			{
				type = ProgramArgumentType.Version;
				return true;
			}

			return false;
		}
	}
}
