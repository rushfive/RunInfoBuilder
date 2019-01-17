using R5.RunInfoBuilder.Configuration.Validators.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Configuration.Validators
{
	internal static class DefaultCommandValidator
	{
		internal static void Validate<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			int commandLevel = -1;

			if (defaultCommand.Arguments != null)
			{
				CommandRules.Common.ArgumentsCannotBeNull(defaultCommand, commandLevel);

				foreach (ArgumentBase<TRunInfo> argument in defaultCommand.Arguments)
				{
					argument.ValidateArg(commandLevel);
				}
			}

			if (defaultCommand.Options != null)
			{
				CommandRules.Common.OptionsCannotBeNull(defaultCommand.Options, commandLevel);
				CommandRules.Common.OptionKeysMustMatchRegex(defaultCommand.Options, commandLevel);
				CommandRules.Common.OptionKeysMustBeUnique(defaultCommand.Options, null, commandLevel);

				foreach (OptionBase<TRunInfo> option in defaultCommand.Options)
				{
					option.ValidateOption(commandLevel);
				}
			}
		}
	}
}
