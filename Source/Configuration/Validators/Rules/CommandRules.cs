using R5.RunInfoBuilder.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Configuration.Validators.Rules
{
	internal static class CommandRules
	{
		internal static class SubCommand
		{
			internal static void SubCommandsCannotBeNull<TRunInfo>(StackableCommand<TRunInfo> command,
				int commandLevel) where TRunInfo : class
			{
				int nullIndex = command.SubCommands.IndexOfFirstNull();
				if (nullIndex != -1)
				{
					throw new CommandValidationException(
						$"Command contains a null subcommand (index {nullIndex}).",
						CommandValidationError.NullObject, commandLevel, nullIndex);
				}
			}

			internal static void SubCommandKeysMustBeUnique<TRunInfo>(StackableCommand<TRunInfo> command,
				int commandLevel) where TRunInfo : class
			{
				bool hasDuplicate = command.SubCommands.Count != command.SubCommands.Select(c => c.Key).Distinct().Count();
				if (hasDuplicate)
				{
					throw new CommandValidationException("Command key is invalid because "
						+ "it clashes with an already configured key.",
						CommandValidationError.DuplicateKey, commandLevel);
				}
			}

			//internal static void SubCommandsAreValid<TRunInfo>(Command<TRunInfo> command,
			//	List<OptionBase<TRunInfo>> globalOptions, int commandLevel)
			//	where TRunInfo : class
			//{
			//	//command.SubCommands.ForEach(c => c.Validate(++commandLevel));
			//	command.SubCommands.ForEach(c => c.ValidateSub(++commandLevel, globalOptions));
			//}
		}

		internal static class Common
		{
			internal static void KeyIsNotNullOrEmpty<TRunInfo>(StackableCommand<TRunInfo> command,
				int commandLevel) where TRunInfo : class
			{
				if (string.IsNullOrWhiteSpace(command.Key))
				{
					throw new CommandValidationException("Command key must be provided.",
						CommandValidationError.KeyNotProvided, commandLevel);
				}
			}

			internal static void ArgumentsCannotBeNull<TRunInfo>(CommandBase<TRunInfo> command,
				int commandLevel) where TRunInfo : class
			{
				int nullIndex = command.Arguments.IndexOfFirstNull();
				if (nullIndex != -1)
				{
					throw new CommandValidationException(
						$"Command contains a null argument (index {nullIndex}).",
						CommandValidationError.NullObject, commandLevel, nullIndex);
				}
			}

			//internal static void ArgumentsAreValid<TRunInfo>(CommandBase<TRunInfo> command,
			//	int commandLevel) where TRunInfo : class
			//{
			//	command.Arguments.ForEach(a => a.Validate(commandLevel));
			//}

			internal static void OptionsCannotBeNull<TRunInfo>(List<OptionBase<TRunInfo>> options,
				int commandLevel) where TRunInfo : class
			{
				int nullIndex = options.IndexOfFirstNull();
				if (nullIndex != -1)
				{
					throw new CommandValidationException(
						$"Command contains a null option (index {nullIndex}).",
						CommandValidationError.NullObject, commandLevel, nullIndex);
				}
			}

			internal static void OptionKeysMustMatchRegex<TRunInfo>(List<OptionBase<TRunInfo>> options,
				int commandLevel) where TRunInfo : class
			{
				bool matchesRegex = options
						.Select(o => o.Key)
						.All(OptionTokenizer.IsValidConfiguration);

				if (!matchesRegex)
				{
					throw new CommandValidationException("Command contains an option with an invalid key.",
						CommandValidationError.InvalidKey, commandLevel);
				}
			}

			internal static void OptionKeysMustBeUnique<TRunInfo>(List<OptionBase<TRunInfo>> options,
				List<OptionBase<TRunInfo>> otherOptions, int commandLevel)
				where TRunInfo : class
			{
				var fullKeys = new List<string>();
				var shortKeys = new List<char>();

				if (otherOptions != null)
				{
					options = options.Concat(otherOptions).ToList();
				}

				options.ForEach(o =>
				{
					var (fullKey, shortKey) = OptionTokenizer.TokenizeKeyConfiguration(o.Key);

					fullKeys.Add(fullKey);

					if (shortKey.HasValue)
					{
						shortKeys.Add(shortKey.Value);
					}
				});

				bool duplicateFull = fullKeys.Count != fullKeys.Distinct().Count();
				if (duplicateFull)
				{
					throw new CommandValidationException("Command contains options with duplicate full keys.",
						CommandValidationError.DuplicateKey, commandLevel);
				}

				bool duplicateShort = shortKeys.Count != shortKeys.Distinct().Count();
				if (duplicateShort)
				{
					throw new CommandValidationException("Command contains options with duplicate short keys.",
						CommandValidationError.DuplicateKey, commandLevel);
				}
			}

			//internal static void OptionsAreValid<TRunInfo>(List<OptionBase<TRunInfo>> options,
			//	int commandLevel) where TRunInfo : class
			//{
			//	options.ForEach(o => o.Validate(commandLevel));
			//}
		}
	}
}
