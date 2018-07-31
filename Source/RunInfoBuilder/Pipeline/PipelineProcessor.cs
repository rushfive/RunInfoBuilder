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
		void ProcessArgs(List<ProgramArgument> programArgumentInfos);
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
			IArgumentMetadata<TRunInfo> argumentMaps,
			RunInfo<TRunInfo> runInfo,
			IParser parser,
			IArgumentTokenizer tokenizer,
			ProcessConfig processConfig,
			ProcessHooksConfig<TRunInfo> hooksConfig)
		{
			_runInfo = runInfo;
			_processConfig = processConfig;

			_pipeline = new List<ProcessPipelineStageBase<TRunInfo>>
			{
				new CommandStage<TRunInfo>(argumentMaps, runInfo),
				new OptionStage<TRunInfo>(argumentMaps, runInfo, tokenizer),
				new ArgumentStage<TRunInfo>(argumentMaps, runInfo, parser, tokenizer)
			};

			Configure(hooksConfig);
		}

		private void Configure(ProcessHooksConfig<TRunInfo> config)
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
		}

		public void ProcessArgs(List<ProgramArgument> programArguments)
		{
			if (_preProcessCallback != null)
			{
				var preProcessContext = new PreProcessContext<TRunInfo>(GetProgramArgumentsFrom(programArguments), _runInfo.Value);
				_preProcessCallback(preProcessContext);
			}

			for (int i = 0; i < programArguments.Count; i++)
			{
				ProgramArgument info = programArguments[i];

				if (info.Type == ProgramArgumentType.Unresolved)
				{
					ThrowIfUnresolvedNotAllowed(info, _processConfig);
					continue;
				}

				ProcessArgumentContext<TRunInfo> stageContext = GetStageContext(info, GetProgramArgumentsFrom(programArguments), _runInfo.Value);

				(int skipNext, AfterProcessingArgument afterArgument) = this.ProcessArgumentInPipeline(
					info.ArgumentToken, stageContext);

				i += skipNext;

				if (afterArgument == AfterProcessingArgument.KillBuild)
				{
					break;
				}
			}

			if (_postProcessCallback != null)
			{
				var postProcessContext = new PostProcessContext<TRunInfo>(GetProgramArgumentsFrom(programArguments), _runInfo.Value);
				_postProcessCallback(postProcessContext);
			}
		}

		// the program arguments made available to users are essentially "immutable"
		// by giving a fresh copy at each accessible place
		private static string[] GetProgramArgumentsFrom(List<ProgramArgument> infos) => infos
				.Select(i => i.ArgumentToken)
				.ToArray();

		private static void ThrowIfUnresolvedNotAllowed(ProgramArgument info, ProcessConfig config)
		{
			switch (config.HandleUnresolved)
			{
				case HandleUnresolvedArgument.NotAllowed:
					// should never reach here due to validation before this
					throw new InvalidOperationException("Unresolved program arguments are invalid for this configuration.");
				case HandleUnresolvedArgument.AllowedButThrowOnProcess:
					throw new RunInfoBuilderException($"Failed to process program argument '{info.ArgumentToken}' because it's an unknown type.");
				case HandleUnresolvedArgument.AllowedButSkipOnProcess:
					break;
				default:
					throw new ArgumentOutOfRangeException($"'{config.HandleUnresolved}' is invalid.");
			}
		}

		private static ProcessArgumentContext<TRunInfo> GetStageContext(ProgramArgument info,
			string[] programArguments, TRunInfo runInfo) => new ProcessArgumentContext<TRunInfo>(
				info.ArgumentToken,
				info.Type,
				info.Position,
				programArguments,
				runInfo);

		private (int SkipNext, AfterProcessingArgument AfterArgument) ProcessArgumentInPipeline(string argumentToken,
			ProcessArgumentContext<TRunInfo> processArgumentContext)
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
	}
}
