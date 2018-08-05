using OLD.ArgumentParser;
using OLD.Configuration;
using OLD.Help;
using OLD.Store;
using OLD.Version;
using System.Collections.Generic;
using System.Linq;

namespace OLD.Process
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
			Queue<StageChain<TRunInfo>> stages = GetOrderedStages();

			return GetLinkedStages(stages);
		}

		private Queue<StageChain<TRunInfo>> GetOrderedStages()
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

			stages.Enqueue(new HandleUnresolvedStage<TRunInfo>(_processConfig));

			stages.Enqueue(new ValidateArgumentStage<TRunInfo>(_argumentMetadata, _parser));
			stages.Enqueue(new ArgumentStage<TRunInfo>(_argumentMetadata, _parser, _runInfo, _tokenizer));

			stages.Enqueue(new ValidateCommandStage<TRunInfo>());
			stages.Enqueue(new CommandStage<TRunInfo>(_argumentMetadata, _runInfo));

			stages.Enqueue(new ValidateOptionStage<TRunInfo>());
			stages.Enqueue(new OptionStage<TRunInfo>(_argumentMetadata, _runInfo, _tokenizer));

			if (_hooksConfig.PostArgumentCallback != null)
			{
				stages.Enqueue(new PostProcessStage<TRunInfo>(_hooksConfig.PostArgumentCallback));
			}

			return stages;
		}

		private StageChain<TRunInfo> GetLinkedStages(Queue<StageChain<TRunInfo>> stages)
		{
			StageChain<TRunInfo> first = stages.Peek();
			StageChain<TRunInfo> prev = stages.Dequeue();

			while (stages.Any())
			{
				var next = stages.Dequeue();
				prev.SetNext(next);
				prev = next;
			}

			return first;
		}
	}
}
