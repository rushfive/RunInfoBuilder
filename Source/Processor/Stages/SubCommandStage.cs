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
		private Action<Queue<Stage<TRunInfo>>> _extendPipelineCallback { get; }

		internal SubCommandStage(
			Dictionary<string, Queue<Stage<TRunInfo>>> subCommandPipelineMap)
			//Action<Queue<Stage<TRunInfo>>> extendPipelineCallback)
		{
			_subCommandPipelineMap = subCommandPipelineMap;
			//_extendPipelineCallback = extendPipelineCallback;
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

			_extendPipelineCallback(pipeline);

			return ProcessResult.Continue;
		}
	}
}
