using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Processor.Models
{
    internal class StageFunctions<TRunInfo>
		where TRunInfo : class
    {
		internal Func<bool> HasMore { get; }
		internal Func<Stage<TRunInfo>> Dequeue { get; }
		internal Action<Queue<Stage<TRunInfo>>> ExtendPipelineWith { get; }

		internal StageFunctions(
			Func<bool> hasMoreFunc,
			Func<Stage<TRunInfo>> dequeueFunc,
			Action<Queue<Stage<TRunInfo>>> extendFunc)
		{
			HasMore = hasMoreFunc;
			Dequeue = dequeueFunc;
			ExtendPipelineWith = extendFunc;
		}
	}
}
