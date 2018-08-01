using R5.RunInfoBuilder.Configuration;
using System;

namespace R5.RunInfoBuilder.Process
{
	internal class ValidateCommandStage<TRunInfo> : ValidationStage<TRunInfo>
		where TRunInfo : class
	{
		internal ValidateCommandStage()
			: base(handlesType: ProgramArgumentType.Command)
		{
		}

		protected override void Validate(ProgramArgument argument, ValidationContext validationContext)
		{
			ThrowIfInvalidPositioning(validationContext, argument.ArgumentToken);
		}

		protected override void MarkSeen(string token, ValidationContext validationContext)
			=> validationContext.MarkCommandSeen(token);

		private void ThrowIfInvalidPositioning(ValidationContext validationContext, string commandToken)
		{
			if (validationContext.CommandConfig.Positioning == CommandPositioning.Front
				&& (validationContext.ArgumentSeen || validationContext.OptionSeen))
			{
				throw new InvalidOperationException($"Command '{commandToken}' is positioned incorrectly. "
					+ "Arguments/Options were found to be before this command.");
			}
		}
	}
}
