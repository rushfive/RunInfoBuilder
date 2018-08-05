using OLD.Process;
using System;

namespace OLD.Configuration
{
	internal class HooksConfig<TRunInfo>
		where TRunInfo : class
	{
		internal Action<BuildContext<TRunInfo>> PreBuildCallback { get; }
		internal Action<BuildContext<TRunInfo>> PostBuildCallback { get; }
		internal Func<ProcessContext<TRunInfo>, ProcessStageResult> PreArgumentCallback { get; }
		internal Func<ProcessContext<TRunInfo>, ProcessStageResult> PostArgumentCallback { get; }

		internal HooksConfig(
			Action<BuildContext<TRunInfo>> preBuildCallback,
			Action<BuildContext<TRunInfo>> postBuildCallback,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> preArgumentCallback,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> postArgumentCallback)
		{
			PreBuildCallback = preBuildCallback;
			PostBuildCallback = postBuildCallback;
			PreArgumentCallback = preArgumentCallback;
			PostArgumentCallback = postArgumentCallback;
		}
	}
}
