﻿using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Version;
using System.Collections.Generic;

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
		private IHelpManager<TRunInfo> _helpManager { get; }
		private HooksConfig<TRunInfo> _hooksConfig { get; }
		private IParser _parser { get; }
		private ProcessConfig _processConfig { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private IArgumentTokenizer _tokenizer { get; }
		private IVersionManager _versionManager { get; }

		public StageChainFactory(
			IArgumentMetadata<TRunInfo> argumentMetadata,
			IHelpManager<TRunInfo> helpManager,
			HooksConfig<TRunInfo> hooksConfig,
			IParser parser,
			ProcessConfig processConfig,
			RunInfo<TRunInfo> runInfo,
			IArgumentTokenizer tokenizer,
			IVersionManager versionManager)
		{
			_argumentMetadata = argumentMetadata;
			_helpManager = helpManager;
			_hooksConfig = hooksConfig;
			_parser = parser;
			_processConfig = processConfig;
			_runInfo = runInfo;
			_tokenizer = tokenizer;
			_versionManager = versionManager;
		}

		public StageChain<TRunInfo> Create()
		{
			var stages = new Queue<StageChain<TRunInfo>>();

			if (_hooksConfig.PreArgumentCallback != null)
			{
				stages.Enqueue(new PreProcessStage<TRunInfo>(_hooksConfig.PreArgumentCallback));
			}

			if (_helpManager != null)
			{
				stages.Enqueue(new HelpStage<TRunInfo>(_helpManager));
			}

			if (_versionManager != null)
			{
				stages.Enqueue(new VersionStage<TRunInfo>(_versionManager));
			}

			var unresolvedstage = new HandleUnresolvedStage<TRunInfo>(_processConfig);
			var argumentStage = new ArgumentStage<TRunInfo>(_argumentMetadata, _parser, _runInfo, _tokenizer);
			var commandStage = new CommandStage<TRunInfo>(_argumentMetadata, _runInfo);
			var optionStage = new OptionStage<TRunInfo>(_argumentMetadata, _runInfo, _tokenizer);

			unresolvedstage
				.SetNext(argumentStage)
				.SetNext(commandStage)
				.SetNext(optionStage);

			StageChain<TRunInfo> first = unresolvedstage;

			

			if (_hooksConfig.PostArgumentCallback != null)
			{
				var postProcessStage = new PostProcessStage<TRunInfo>(_hooksConfig.PreArgumentCallback);
				optionStage.SetNext(postProcessStage);
			}

			return first;
		}
	}
}
