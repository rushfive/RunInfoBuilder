using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using R5.RunInfoBuilder.Commands;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class SubCommandStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Dictionary<string, (Queue<Stage<TRunInfo>>, Command<TRunInfo>)> _subCommandInfoMap { get; }

		internal SubCommandStage(
			Dictionary<string, (Queue<Stage<TRunInfo>>, Command<TRunInfo>)> subCommandInfoMap)
		{
			_subCommandInfoMap = subCommandInfoMap;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
			if (!context.ProgramArguments.HasMore())
			{
				return ProcessResult.End;
			}

			string subCommand = context.ProgramArguments.Dequeue(context.CommandLevel);

			if (!_subCommandInfoMap.TryGetValue(subCommand, out (Queue<Stage<TRunInfo>>, Command<TRunInfo>) subCommandInfo))
			{
				throw new ProcessException($"'{subCommand}' is not a valid sub command.",
					ProcessError.InvalidSubCommand, context.CommandLevel);
			}

			(Queue<Stage<TRunInfo>> pipeline, Command<TRunInfo> command) = subCommandInfo;

			context.ExtendPipeline(pipeline);

			context.RefreshForCommand(command);

			return ProcessResult.Continue;
		}
	}
}
