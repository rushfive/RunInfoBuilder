using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	public class CustomArgument<TRunInfo> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public int Count { get; set; }
		public Func<CustomHandlerContext<TRunInfo>, ProcessStageResult> Handler { get; set; }

		internal override List<Action<int>> Rules() => ValidationRules.Arguments.Custom.Rules(this);

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
