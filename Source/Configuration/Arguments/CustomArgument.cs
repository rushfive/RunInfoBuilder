using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// Handles n number of consecutive program arguments using a custom callback.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the argument's associated to.</typeparam>
	public class CustomArgument<TRunInfo> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		/// <summary>
		/// The number of program arguments handled.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// The custom callback that handles the program arguments.
		/// </summary>
		public Func<CustomHandlerContext<TRunInfo>, ProcessStageResult> Handler { get; set; }

		internal override List<Action<int>> Rules() => ValidationRules.Arguments.Custom.Rules(this);

		internal override Stage<TRunInfo> ToStage()
		{
			return new CustomArgumentStage<TRunInfo>(Count, Handler);
		}

		internal override string GetHelpToken()
		{
			return string.IsNullOrWhiteSpace(HelpToken) ? "<custom>" : HelpToken;
		}
	}
}
