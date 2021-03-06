﻿using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class SubCommandStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Dictionary<string, (Queue<Stage<TRunInfo>>, SubCommand<TRunInfo>)> _subCommandInfoMap { get; }

		internal SubCommandStage(
			Dictionary<string, (Queue<Stage<TRunInfo>>, SubCommand<TRunInfo>)> subCommandInfoMap)
		{
			_subCommandInfoMap = subCommandInfoMap;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context, 
			Action<CommandBase<TRunInfo>> resetContextFunc)
		{
			if (!context.ProgramArguments.HasMore())
			{
				throw new ProcessException("Expected an argument but reached the end of program args.",
					ProcessError.ExpectedProgramArgument, context.CommandLevel);
			}

			string subCommand = context.ProgramArguments.Dequeue();

			if (!_subCommandInfoMap.TryGetValue(subCommand, out (Queue<Stage<TRunInfo>>, SubCommand<TRunInfo>) subCommandInfo))
			{
				throw new ProcessException($"'{subCommand}' is not a valid sub command.",
					ProcessError.InvalidSubCommand, context.CommandLevel);
			}

			(Queue<Stage<TRunInfo>> subCommandStages, SubCommand<TRunInfo> command) = subCommandInfo;

			ProcessStageResult onMatchedResult = command.OnMatched?.Invoke(context.RunInfo);
			if (onMatchedResult == ProcessResult.End)
			{
				return ProcessResult.End;
			}

			context.Stages.ExtendPipelineWith(subCommandStages);
			
			resetContextFunc(command);

			return ProcessResult.Continue;
		}
	}
}
