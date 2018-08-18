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
		private ProcessContext<TRunInfo> _context { get; }

		internal CallbackStage(
			Func<CallbackContext<TRunInfo>, ProcessStageResult> callback,
			ProcessContext<TRunInfo> context)
			: base(context)
		{
			_callback = callback;
			_context = context;
		}

		internal override ProcessStageResult ProcessStage()
		{
			throw new NotImplementedException("TODO!!!!");
		}
	}
}
