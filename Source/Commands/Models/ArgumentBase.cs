using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public abstract class ArgumentBase<TRunInfo> : ICallbackElement<TRunInfo>
		where TRunInfo : class
	{
		public Func<CallbackContext<TRunInfo>, ProcessStageResult> Callback { get; set; }
		public string HelpText { get; set; }

		internal abstract void Validate(Type parentType, string parentKey);

		internal abstract Stage<TRunInfo> ToStage(IArgumentParser parser);
	}
}
