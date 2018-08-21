using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class CallbackStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Func<CallbackContext<TRunInfo>, ProcessStageResult> _callback { get; }

		internal CallbackStage(Func<CallbackContext<TRunInfo>, ProcessStageResult> callback)
		{
			_callback = callback;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
			CallbackContext<TRunInfo> context = callbackContextFactory();

			return _callback(context);
		}
	}
}
