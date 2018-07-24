using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Pipeline
{
	internal abstract class ProcessPipelineStageBase<TRunInfo>
		where TRunInfo : class
	{
		private ProgramArgumentType? _handlesArgumentType { get; set; }
		private bool _handlesAnyType => !this._handlesArgumentType.HasValue;

		protected ProcessPipelineStageBase(ProgramArgumentType? handlesArgumentType)
		{
			this._handlesArgumentType = handlesArgumentType;
		}

		internal abstract ProcessStageResult Process(ProcessArgumentContext<TRunInfo> context);

		internal bool CanProcessArgument(ProgramArgumentType argumentType)
		{
			if (_handlesAnyType)
			{
				return true;
			}
			return this._handlesArgumentType.Value == argumentType;
		}
	}
}
