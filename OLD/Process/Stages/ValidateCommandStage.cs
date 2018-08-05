using OLD.Configuration;
using System;

namespace OLD.Process
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
			var token = argument.ArgumentToken;

			ThrowIfOtherCommandAlreadySeen(token, validationContext);

			ThrowIfPositionedInvalid(token, validationContext);
		}

		protected override void MarkSeen(string token, ValidationContext validationContext)
			=> validationContext.MarkCommandSeen(token);

		private void ThrowIfOtherCommandAlreadySeen(string token, ValidationContext validationContext)
		{
			if (validationContext.CommandConfig.EnforceSingleCommand)
			{
				throw new InvalidOperationException($"Command '{token}' is invalid because processing "
					+ "is configured to only have a single command, but another was already seen.");
			}
		}

		private void ThrowIfPositionedInvalid(string token, ValidationContext validationContext)
		{
			if (validationContext.CommandConfig.Positioning == CommandPositioning.Front
				&& (validationContext.ArgumentSeen || validationContext.OptionSeen))
			{
				throw new InvalidOperationException($"Command '{token}' is positioned incorrectly. "
					+ "Arguments/Options were found to be before this command.");
			}
		}
	}
}
