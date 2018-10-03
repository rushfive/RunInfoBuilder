using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class CustomArgumentStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private int _count { get; }
		private Func<CustomHandlerContext<TRunInfo>, ProcessStageResult> _handler { get; }

		internal CustomArgumentStage(
			int count,
			Func<CustomHandlerContext<TRunInfo>, ProcessStageResult> handler)
		{
			_count = count;
			_handler = handler;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context,
			Action<CommandBase<TRunInfo>> resetContextFunc)
		{
			CustomHandlerContext<TRunInfo> handlerContext = GetCustomHandlerContext(context);

			return _handler(handlerContext);
		}

		private CustomHandlerContext<TRunInfo> GetCustomHandlerContext(ProcessContext<TRunInfo> context)
		{
			var handledArguments = new List<string>();

			int count = _count;
			while (count > 0)
			{
				if (!context.ProgramArguments.HasMore())
				{
					throw new ProcessException(
						"Reached the end of program arguments while processing the custom argument stage.",
						ProcessError.ExpectedProgramArgument, context.CommandLevel);
				}

				handledArguments.Add(context.ProgramArguments.Dequeue());
				count--;
			}

			return new CustomHandlerContext<TRunInfo>(context.RunInfo, handledArguments, context.Parser);
		}
	}
}
