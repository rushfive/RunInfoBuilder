using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Processor.Models;
using System;

namespace R5.RunInfoBuilder.Processor.Stages
{
    internal abstract class Stage<TRunInfo>
		where TRunInfo : class
    {
		protected Stage() { }
		
		internal abstract ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context, Action<CommandBase<TRunInfo>> resetContextFunc);
	}
}
