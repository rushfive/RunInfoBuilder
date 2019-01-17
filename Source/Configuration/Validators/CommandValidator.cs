using R5.RunInfoBuilder.Configuration.Validators.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Configuration.Validators
{
	internal static class CommandValidator
	{
		internal static void Validate<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
		{
			CommandRules.Common.KeyIsNotNullOrEmpty(command, 0);

			if (command.Arguments != null)
			{
				CommandRules.Common.ArgumentsCannotBeNull(command, 0);

				foreach (ArgumentBase<TRunInfo> argument in command.Arguments)
				{
					argument.ValidateArg(0);
				}
			}

			if (command.Options != null)
			{
				CommandRules.Common.OptionsCannotBeNull(command.Options, 0);
				CommandRules.Common.OptionKeysMustMatchRegex(command.Options, 0);
				CommandRules.Common.OptionKeysMustBeUnique(command.Options, command.GlobalOptions, 0);

				foreach (OptionBase<TRunInfo> option in command.Options)
				{
					option.ValidateOption(0);
				}
			}

			if (command.GlobalOptions != null)
			{
				CommandRules.Common.OptionsCannotBeNull(command.GlobalOptions, 0);
				CommandRules.Common.OptionKeysMustMatchRegex(command.GlobalOptions, 0);
				CommandRules.Common.OptionKeysMustBeUnique(command.GlobalOptions, null, 0);

				foreach (OptionBase<TRunInfo> option in command.GlobalOptions)
				{
					option.ValidateOption(0);
				}
			}

			if (command.SubCommands != null)
			{
				CommandRules.SubCommand.SubCommandsCannotBeNull(command, 0);
				CommandRules.SubCommand.SubCommandKeysMustBeUnique(command, 0);

				foreach (SubCommand<TRunInfo> subCommand in command.SubCommands)
				{
					ValidateSubCommand(subCommand, command.GlobalOptions, 1);
				}
			}
		}

		private static void ValidateSubCommand<TRunInfo>(SubCommand<TRunInfo> subCommand,
			List<OptionBase<TRunInfo>> globalOptions, int commandLevel) where TRunInfo : class
		{
			CommandRules.Common.KeyIsNotNullOrEmpty(subCommand, commandLevel);

			if (subCommand.Arguments != null)
			{
				CommandRules.Common.ArgumentsCannotBeNull(subCommand, commandLevel);

				foreach (ArgumentBase<TRunInfo> argument in subCommand.Arguments)
				{
					argument.ValidateArg(commandLevel);
				}
			}

			if (subCommand.Options != null)
			{
				CommandRules.Common.OptionsCannotBeNull(subCommand.Options, commandLevel);
				CommandRules.Common.OptionKeysMustMatchRegex(subCommand.Options, commandLevel);
				CommandRules.Common.OptionKeysMustBeUnique(subCommand.Options, globalOptions, commandLevel);

				foreach (OptionBase<TRunInfo> option in subCommand.Options)
				{
					option.ValidateOption(commandLevel);
				}
			}

			if (subCommand.SubCommands != null)
			{
				CommandRules.SubCommand.SubCommandsCannotBeNull(subCommand, commandLevel);
				CommandRules.SubCommand.SubCommandKeysMustBeUnique(subCommand, commandLevel);

				foreach (SubCommand<TRunInfo> sc in subCommand.SubCommands)
				{
					ValidateSubCommand(sc, globalOptions, commandLevel + 1);
				}
			}
		}
	}
}
