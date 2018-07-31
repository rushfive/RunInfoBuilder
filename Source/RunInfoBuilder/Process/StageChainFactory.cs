using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Store;

namespace R5.RunInfoBuilder.Process
{
	internal interface IStageChainFactory<TRunInfo>
		where TRunInfo : class
	{
		StageChain<TRunInfo> Create();
	}

	// Processes arguments using chain of responsibility
	internal class StageChainFactory<TRunInfo> : IStageChainFactory<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentMetadata<TRunInfo> _argumentMetadata { get; }
		private HooksConfig<TRunInfo> _hooksConfig { get; }
		private IParser _parser { get; }
		private ProcessConfig _processConfig { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private IArgumentTokenizer _tokenizer { get; }

		public StageChainFactory(
			IArgumentMetadata<TRunInfo> argumentMetadata,
			HooksConfig<TRunInfo> hooksConfig,
			IParser parser,
			ProcessConfig processConfig,
			RunInfo<TRunInfo> runInfo,
			IArgumentTokenizer tokenizer)
		{
			_argumentMetadata = argumentMetadata;
			_hooksConfig = hooksConfig;
			_parser = parser;
			_processConfig = processConfig;
			_runInfo = runInfo;
			_tokenizer = tokenizer;
		}

		public StageChain<TRunInfo> Create()
		{
			var unresolvedstage = new HandleUnresolvedStage<TRunInfo>(_processConfig);
			var argumentStage = new ArgumentStage<TRunInfo>(_argumentMetadata, _parser, _runInfo, _tokenizer);
			var commandStage = new CommandStage<TRunInfo>(_argumentMetadata, _runInfo);
			var optionStage = new OptionStage<TRunInfo>(_argumentMetadata, _runInfo, _tokenizer);

			unresolvedstage
				.SetNext(argumentStage)
				.SetNext(commandStage)
				.SetNext(optionStage);

			StageChain<TRunInfo> first = unresolvedstage;

			if (_hooksConfig.PreArgumentCallback != null)
			{
				var preProcessStage = new PreProcessStage<TRunInfo>(_hooksConfig.PreArgumentCallback);
				preProcessStage.SetNext(unresolvedstage);

				first = preProcessStage;
			}

			if (_hooksConfig.PostArgumentCallback != null)
			{
				var postProcessStage = new PostProcessStage<TRunInfo>(_hooksConfig.PreArgumentCallback);
				optionStage.SetNext(postProcessStage);
			}

			return first;
		}
	}
}
