using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class CustomArgument<TRunInfo> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public int Count { get; set; }
		public Func<CustomHandlerContext<TRunInfo>, ProcessStageResult> Handler { get; set; }

		internal override void Validate(int commandLevel)
		{
			if (Count <= 0)
			{
				throw new CommandValidationException("Custom argument has an invalid count. Must be greater than 0.",
					CommandValidationError.InvalidCount, commandLevel);
			}

			if (Handler == null)
			{
				throw new CommandValidationException("Custom argument is missing its handler callback.",
					CommandValidationError.NullCustomHandler, commandLevel);
			}

			if (string.IsNullOrWhiteSpace(HelpToken))
			{
				throw new CommandValidationException("Custom arguments must explicitly set their own help token string.",
					CommandValidationError.NullHelpToken, commandLevel);
			}
		}

		internal override Stage<TRunInfo> ToStage()
		{
			return new CustomArgumentStage<TRunInfo>(Count, Handler);
		}

		internal override string GetHelpToken()
		{
			return HelpToken;
		}
	}
}
