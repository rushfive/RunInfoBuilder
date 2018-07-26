using R5.RunInfoBuilder.ArgumentParser;
using System;
using System.Collections.Generic;
using System.Linq;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Configuration;

namespace R5.RunInfoBuilder.Pipeline
{
	internal interface IPipelineProcessor<TRunInfo>
		where TRunInfo : class
	{
		void ProcessArgs(List<ProgramArgumentInfo> programArgumentInfos);
	}

	internal class PipelineProcessor<TRunInfo> : IPipelineProcessor<TRunInfo>
		where TRunInfo : class
	{
		enum AfterProcessingArgument
		{
			Continue,
			KillBuild
		}

		private RunInfo<TRunInfo> _runInfo { get; }
		private ProcessConfig _processConfig { get; }
		private List<ProcessPipelineStageBase<TRunInfo>> _pipeline { get; set; }
		private Action<PreProcessContext<TRunInfo>> _preProcessCallback { get; set; }
		private Action<PostProcessContext<TRunInfo>> _postProcessCallback { get; set; }

		public PipelineProcessor(
			IArgumentMetadataMaps<TRunInfo> argumentMaps,
			RunInfo<TRunInfo> runInfo,
			IParser parser,
			IArgumentTokenizer tokenizer,
			ProcessConfig processConfig)
		{
			_runInfo = runInfo;
			_processConfig = processConfig;

			_pipeline = new List<ProcessPipelineStageBase<TRunInfo>>
			{
				new CommandStage<TRunInfo>(argumentMaps, runInfo),
				new OptionStage<TRunInfo>(argumentMaps, runInfo, tokenizer),
				new ArgumentStage<TRunInfo>(argumentMaps, runInfo, parser, tokenizer)
			};
		}

		public PipelineProcessor<TRunInfo> Configure(ProcessHooksConfig<TRunInfo> config)
		{
			_preProcessCallback = config.PreProcessCallback;
			_postProcessCallback = config.PostProcessCallback;

			if (config.PreArgumentCallback != null)
			{
				_pipeline.Insert(0, new PreProcessArgumentStage<TRunInfo>(config.PreArgumentCallback));
			}

			if (config.PostArgumentCallback != null)
			{
				_pipeline.Add(new PostProcessArgumentStage<TRunInfo>(config.PostArgumentCallback));
			}

			return this;
		}

		public void ProcessArgs(List<ProgramArgumentInfo> programArgumentInfos)
		{
			if (_preProcessCallback != null)
			{
				var preProcessContext = new PreProcessContext<TRunInfo>(getProgramArguments(), _runInfo.Value);
				_preProcessCallback(preProcessContext);
			}

			for (int i = 0; i < programArgumentInfos.Count; i++)
			{
				ProgramArgumentInfo info = programArgumentInfos[i];
				
				if (info.Type == ProgramArgumentType.Unresolved)
				{
					switch (_processConfig.HandleUnresolvedArgument)
					{
						case HandleUnresolvedArgument.NotAllowed:
							// should never reach here due to validation before this
							throw new InvalidOperationException("Unresolved program arguments are invalid for this configuration.");
						case HandleUnresolvedArgument.AllowedButThrowOnProcess:
							throw new RunInfoBuilderException($"Failed to process program argument '{info.RawArgumentToken}' because it's an unknown type.");
						case HandleUnresolvedArgument.AllowedButSkipOnProcess:
							continue;
						default:
							throw new ArgumentOutOfRangeException($"'{_processConfig.HandleUnresolvedArgument}' is invalid.");
					}
				}

				var stageContext = new ProcessArgumentContext<TRunInfo>(
					info.RawArgumentToken, 
					info.Type,
					info.Position, 
					getProgramArguments(), 
					_runInfo.Value);

				(int skipNext, AfterProcessingArgument afterArgument) = this.ProcessArgumentInPipeline(info.RawArgumentToken, stageContext, ref i);

				i += skipNext;

				if (afterArgument == AfterProcessingArgument.KillBuild)
				{
					break;
				}
			}

			if (_postProcessCallback != null)
			{
				var postProcessContext = new PostProcessContext<TRunInfo>(getProgramArguments(), _runInfo.Value);
				_postProcessCallback(postProcessContext);
			}

			// the program arguments made available to users are essentially "immutable"
			// by giving a fresh copy at each accessible place
			string[] getProgramArguments() => programArgumentInfos
				.Select(i => i.RawArgumentToken)
				.ToArray();
		}

		private (int SkipNext, AfterProcessingArgument AfterArgument) ProcessArgumentInPipeline(string argumentToken, 
			ProcessArgumentContext<TRunInfo> processArgumentContext, ref int i)
		{
			int totalSkipNext = 0;
			foreach (ProcessPipelineStageBase<TRunInfo> stage in _pipeline)
			{
				bool shouldProcess = stage.CanProcessArgument(processArgumentContext.ArgumentType);
				if (!shouldProcess)
				{
					continue;
				}

				(int skipNext, AfterProcessingStage afterStage) = stage.Process(processArgumentContext);

				// always increments i by skip (note this in docs!, potentially edgy weird cases where multiple callbacks increment!)
				//i += skipNext;
				totalSkipNext += skipNext;

				if (afterStage == AfterProcessingStage.StopProcessingRemainingStages)
				{
					break;
				}
				if (afterStage == AfterProcessingStage.KillBuild)
				{
					return (skipNext, AfterProcessingArgument.KillBuild);
				}
			}

			return (totalSkipNext, AfterProcessingArgument.Continue);
		}

		// TODO: remove AFTER finishing new unit tests
		//internal List<Type> GetPipelineStageTypes()
		//{
		//	var result = new List<Type>();
		//	foreach(ProcessPipelineStageBase<TRunInfo> stage in _pipeline)
		//	{
		//		result.Add(stage.GetType());
		//	}
		//	return result;
		//}
	}
}
