using R5.RunInfoBuilder.Configuration.Validators.Rules;
using R5.RunInfoBuilder.Processor.Stages;
using System;

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

		internal override Stage<TRunInfo> ToStage()
		{
			return new CustomArgumentStage<TRunInfo>(Count, Handler);
		}

		internal override string GetHelpToken()
		{
			return string.IsNullOrWhiteSpace(HelpToken) ? "<custom>" : HelpToken;
		}

		internal override void Validate(int commandLevel)
		{
			ArgumentRules.Custom.CountMustBeGreaterThanZero(this, commandLevel);
			ArgumentRules.Custom.HandlerMustBeSet(this, commandLevel);
		}
	}
}
