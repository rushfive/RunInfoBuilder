using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Process;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Validators;
using R5.RunInfoBuilder.Version;

namespace R5.RunInfoBuilder.Configuration
{
	internal class RunInfoBuilderDependencies<TRunInfo>
			where TRunInfo : class
	{
		internal IProcessInvoker ProcessInvoker { get; }
		internal IParser Parser { get; }
		internal IArgumentStore<TRunInfo> Store { get; }
		internal IBuildValidator BuildValidator { get; }
		internal RunInfo<TRunInfo> RunInfo { get; }
		internal BuilderConfig Config { get; }
		internal HooksConfig<TRunInfo> HooksConfig { get; }

		internal RunInfoBuilderDependencies(
			IProcessInvoker processInvoker,
			IParser parser,
			IArgumentStore<TRunInfo> store,
			IBuildValidator buildValidator,
			RunInfo<TRunInfo> runInfo,
			BuilderConfig config,
			HooksConfig<TRunInfo> hooksConfig)
		{
			ProcessInvoker = processInvoker;
			Parser = parser;
			Store = store;
			BuildValidator = buildValidator;
			RunInfo = runInfo;
			Config = config;
			HooksConfig = hooksConfig;
		}
	}
}
