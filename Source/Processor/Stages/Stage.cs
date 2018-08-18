using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Stages
{
    internal abstract class Stage<TRunInfo>
		where TRunInfo : class
    {
		protected ProcessContext<TRunInfo> _context { get; }

		protected Stage(
			ProcessContext<TRunInfo> context)
		{
			_context = context;
		}

		// making NOT abstract because options dont require a callback context, but commands
		// and arguments do
		internal abstract ProcessStageResult ProcessStage();

		protected IArgumentParser Parser => _context.Parser;

		protected bool MoreProgramArgumentsExist() => _context.HasNext();

		protected string Peek() => _context.Peek();

		protected string Dequeue() => _context.Dequeue();

		protected bool NextIsSubCommand() => _context.NextIsSubCommand();

		protected bool NextIsOption() => _context.NextIsOption();

		protected TRunInfo RunInfo => _context.RunInfo;
	}
}
