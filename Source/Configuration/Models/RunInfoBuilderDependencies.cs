using OLD.ArgumentParser;
using OLD.Help;
using OLD.Process;
using OLD.Store;
using OLD.Validators;
using OLD.Version;

namespace OLD.Configuration
{
	internal class RunInfoBuilderDependencies<TRunInfo>
			where TRunInfo : class
	{
		internal IProcessInvoker ProcessInvoker { get; }
		internal IParser Parser { get; }
		internal IArgumentStore<TRunInfo> Store { get; }
		internal RunInfo<TRunInfo> RunInfo { get; }
		internal BuilderConfig Config { get; }
		internal HooksConfig<TRunInfo> HooksConfig { get; }

		internal RunInfoBuilderDependencies(
			IProcessInvoker processInvoker,
			IParser parser,
			IArgumentStore<TRunInfo> store,
			RunInfo<TRunInfo> runInfo,
			BuilderConfig config,
			HooksConfig<TRunInfo> hooksConfig)
		{
			ProcessInvoker = processInvoker;
			Parser = parser;
			Store = store;
			RunInfo = runInfo;
			Config = config;
			HooksConfig = hooksConfig;
		}
	}
}
