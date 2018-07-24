using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Validators
{
	internal class CommandsPositionedFrontRule : ValidationRule<ProgramArgumentValidationInfo[]>
	{
		private CommandConfig _config { get; }

		internal CommandsPositionedFrontRule(CommandConfig config)
		{
			_config = config;
		}

		protected override Func<ProgramArgumentValidationInfo[], bool> _validateFunction => argumentInfos =>
		{
			if (_config.Positioning == CommandPositioning.Anywhere)
			{
				return true;
			}

			List<ProgramArgumentValidationInfo> commandInfos = argumentInfos
				.Where(i => i.Type == ProgramArgumentType.Command)
				.ToList();
			
			if (commandInfos.Count == 0)
			{
				return true;
			}

			int greatestAllowedIndex = commandInfos.Count - 1;

			List<ProgramArgumentValidationInfo> invalidCommands = commandInfos
				.Where(i => i.Position > greatestAllowedIndex)
				.ToList();

			if (!invalidCommands.Any())
			{
				return true;
			}

			foreach (ProgramArgumentValidationInfo info in invalidCommands)
			{
				info.AddError("Commands must be positioned at the front.");
			}

			return false;
		};
	}
}
