using R5.RunInfoBuilder.Pipeline;
using R5.RunInfoBuilder.Process;
using System;

namespace R5.RunInfoBuilder.Configuration
{
	internal class ProcessHooksConfig<TRunInfo>
		where TRunInfo : class
	{
		internal Action<PreProcessContext<TRunInfo>> PreProcessCallback { get; }
		internal Action<PostProcessContext<TRunInfo>> PostProcessCallback { get; }
		internal Func<ProcessContext<TRunInfo>, ProcessStageResult> PreArgumentCallback { get; }
		internal Func<ProcessContext<TRunInfo>, ProcessStageResult> PostArgumentCallback { get; }

		internal ProcessHooksConfig(
			Action<PreProcessContext<TRunInfo>> preProcessCallback,
			Action<PostProcessContext<TRunInfo>> postProcessCallback,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> preArgumentCallback,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> postArgumentCallback)
		{
			PreProcessCallback = preProcessCallback;
			PostProcessCallback = postProcessCallback;
			PreArgumentCallback = preArgumentCallback;
			PostArgumentCallback = postArgumentCallback;
		}
	}
}
