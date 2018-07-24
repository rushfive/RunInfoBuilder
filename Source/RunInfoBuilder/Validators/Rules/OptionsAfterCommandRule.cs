using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Validators
{
	internal class OptionsAfterCommandRule : ValidationRule<ProgramArgumentValidationInfo[]>
	{
		private OptionConfig _config { get; }

		internal OptionsAfterCommandRule(OptionConfig config)
		{
			_config = config;
		}

		protected override Func<ProgramArgumentValidationInfo[], bool> _validateFunction => argumentInfos =>
		{
			if (_config.Positioning == ArgumentOptionPositioning.Anywhere)
			{
				return true;
			}

			List<ProgramArgumentValidationInfo> commandInfos = argumentInfos
				.Where(i => i.Type == ProgramArgumentType.Command)
				.ToList();

			if (!commandInfos.Any())
			{
				return true;
			}

			int lastCommandIndex = commandInfos.Max(i => i.Position);

			List<ProgramArgumentValidationInfo> invalidOptions = argumentInfos
				.Where(i => i.Type == ProgramArgumentType.Option && i.Position < lastCommandIndex)
				.ToList();

			if (!invalidOptions.Any())
			{
				return true;
			}

			foreach (ProgramArgumentValidationInfo info in invalidOptions)
			{
				info.AddError("Options must be positioned before any commands.");
			}

			return false;
		};
	}
}
