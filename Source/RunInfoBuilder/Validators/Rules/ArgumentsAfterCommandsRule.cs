//using R5.RunInfoBuilder.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace R5.RunInfoBuilder.Validators
//{
//	internal class ArgumentsAfterCommandsRule : ValidationRule<ProgramArgumentValidationInfo[]>
//	{
//		private ArgumentConfig _config { get; }

//		internal ArgumentsAfterCommandsRule(ArgumentConfig config)
//		{
//			_config = config;
//		}

//		protected override Func<ProgramArgumentValidationInfo[], bool> _validateFunction => argumentInfos =>
//		{
//			if (_config.Positioning == ArgumentOptionPositioning.Anywhere)
//			{
//				return true;
//			}

//			List<ProgramArgumentValidationInfo> commandInfos = argumentInfos
//				.Where(i => i.Type == ProgramArgumentType.Command)
//				.ToList();

//			if (!commandInfos.Any())
//			{
//				return true;
//			}

//			int lastCommandIndex = commandInfos.Max(i => i.Position);

//			List<ProgramArgumentValidationInfo> invalidArguments = argumentInfos
//				.Where(i => i.Type == ProgramArgumentType.Argument && i.Position < lastCommandIndex)
//				.ToList();

//			if (!invalidArguments.Any())
//			{
//				return true;
//			}

//			foreach(ProgramArgumentValidationInfo info in invalidArguments)
//			{
//				info.AddError("Arguments must be positioned before any commands.");
//			}

//			return false;
//		};
//	}
//}
