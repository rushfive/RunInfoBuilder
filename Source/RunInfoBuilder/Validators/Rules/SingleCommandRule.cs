using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using R5.RunInfoBuilder.Configuration;

namespace R5.RunInfoBuilder.Validators
{
	internal class SingleCommandRule : ValidationRule<ProgramArgumentValidationInfo[]>
	{
		private CommandConfig _config { get; }

		internal SingleCommandRule(CommandConfig config)
		{
			_config = config;
		}

		protected override Func<ProgramArgumentValidationInfo[], bool> _validateFunction => argumentInfos =>
		{
			List<ProgramArgumentValidationInfo> commandInfos = argumentInfos
				.Where(i => i.Type == ProgramArgumentType.Command)
				.ToList();

			if (commandInfos.Count <= 1)
			{
				return true;
			}

			foreach(ProgramArgumentValidationInfo info in commandInfos.Skip(1))
			{
				info.AddError("There can only be a single command.");
			}

			return false;
		};
	}
}
