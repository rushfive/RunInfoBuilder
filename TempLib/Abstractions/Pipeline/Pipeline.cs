using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.Lib.Abstractions.Pipeline
{
	public abstract class Pipeline<TContext, TReturn>
	{
		protected TContext context { get; }
		private List<PipelineStage<TContext>> stages { get; }

		protected Pipeline(
			TContext context,
			List<PipelineStage<TContext>> stages)
		{
			this.context = context;
			this.stages = stages;
		}

		// override if your derived pipeline requires different handling
		protected virtual async Task<TReturn> ProcessAsync()
		{
			foreach (PipelineStage<TContext> stage in this.stages)
			{
				ProcessStageResult result = await stage.ProcessAsync(this.context);

				switch (result)
				{
					case ContinueResult _:
						break;
					case ReturnWithValue<TReturn> returnResult:
						return returnResult.Value;
					default:
						throw new ArgumentOutOfRangeException(nameof(result));
				}
			}

			throw new InvalidOperationException("Pipeline finished processing but never returned a value.");
		}
	}
}
