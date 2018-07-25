using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Pipeline;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Validators;
using R5.RunInfoBuilder.Version;

namespace R5.RunInfoBuilder.Configuration
{
	internal class RunInfoBuilderDependencies<TRunInfo>
			where TRunInfo : class
	{
		internal IParser Parser { get; }
		internal IPipelineProcessor<TRunInfo> Pipeline { get; }
		internal IArgumentStore<TRunInfo> Store { get; }
		internal IBuildValidator BuildValidator { get; }
		internal IHelpManager<TRunInfo> HelpManager { get; }
		internal RunInfo<TRunInfo> RunInfo { get; }
		internal IVersionManager VersionManager { get; }
		internal BuilderConfig Config { get; }

		internal RunInfoBuilderDependencies(
			IParser parser,
			IPipelineProcessor<TRunInfo> pipeline,
			IArgumentStore<TRunInfo> store,
			IBuildValidator buildValidator,
			IHelpManager<TRunInfo> helpManager,
			RunInfo<TRunInfo> runInfo,
			IVersionManager versionManager,
			BuilderConfig config)
		{
			Parser = parser;
			Pipeline = pipeline;
			Store = store;
			BuildValidator = buildValidator;
			HelpManager = helpManager;
			RunInfo = runInfo;
			VersionManager = versionManager;
			Config = config;
		}
	}
}
