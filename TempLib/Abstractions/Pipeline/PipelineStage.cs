using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.Lib.Abstractions.Pipeline
{
	public abstract class PipelineStage<TContext>
	{
		protected PipelineStage()
		{
		}

		public abstract Task<ProcessStageResult> ProcessAsync(TContext context);
	}
}
