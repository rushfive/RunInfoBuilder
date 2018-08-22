using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class SubCommandStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Dictionary<string, Queue<Stage<TRunInfo>>> _subCommandPipelineMap { get; }

		internal SubCommandStage(
			Dictionary<string, Queue<Stage<TRunInfo>>> subCommandPipelineMap)
		{
			_subCommandPipelineMap = subCommandPipelineMap;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
			if (!context.ProgramArguments.HasMore())
			{
				return ProcessResult.End;
			}

			string subCommand = context.ProgramArguments.Dequeue();

			if (!_subCommandPipelineMap.TryGetValue(subCommand, out Queue<Stage<TRunInfo>> pipeline))
			{
				throw new InvalidOperationException($"'{subCommand}' is not a valid sub command.");
			}

			context.ExtendPipeline(pipeline);

			return ProcessResult.Continue;
		}
	}
}
