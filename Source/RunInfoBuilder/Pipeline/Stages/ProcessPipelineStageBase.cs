using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Pipeline
{
	internal abstract class ProcessPipelineStageBase<TRunInfo>
		where TRunInfo : class
	{
		private ProgramArgumentType? _handlesArgumentType { get; set; }

		protected ProcessPipelineStageBase(ProgramArgumentType? handlesArgumentType)
		{
			this._handlesArgumentType = handlesArgumentType;
		}

		internal abstract (int SkipNext, AfterProcessingStage AfterStage) Process(ProcessArgumentContext<TRunInfo> context);

		internal bool CanProcessArgument(ProgramArgumentType argumentType)
		{
			bool alwaysProcessed = !this._handlesArgumentType.HasValue;
			if (alwaysProcessed)
			{
				return true;
			}
			return this._handlesArgumentType.Value == argumentType;
		}
	}
}
