using R5.RunInfoBuilder.Store;
using System.Collections.Generic;
using System.Linq;
using System;

namespace R5.RunInfoBuilder.Pipeline
{
	internal class OptionStage<TRunInfo> : ProcessPipelineStageBase<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentMetadataMaps<TRunInfo> _argumentMaps { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private IArgumentTokenizer _tokenizer { get; }

		internal OptionStage(
			IArgumentMetadataMaps<TRunInfo> argumentMaps, 
			RunInfo<TRunInfo> runInfo,
			IArgumentTokenizer tokenizer)
			: base(ProgramArgumentType.Option)
		{
			_argumentMaps = argumentMaps;
			_runInfo = runInfo;
			_tokenizer = tokenizer;
		}

		internal override (int SkipNext, AfterProcessingStage AfterStage) Process(ProcessArgumentContext<TRunInfo> context)
		{
			(OptionType type, string fullKey, List<char> shortKeys) = _tokenizer.TokenizeOption(context.Token);

			switch (type)
			{
				case OptionType.Full:
					ResolveFullOption(fullKey);
					break;
				case OptionType.Short:
					ResolveShortOption(shortKeys.Single());
					break;
				case OptionType.ShortCompound:
					ResolveShortCompoundOption(shortKeys);
					break;
				default:
					throw new ArgumentOutOfRangeException($"'{type}' is not a valid option type.");
			}

			return (0, AfterProcessingStage.Continue);
		}

		private void ResolveFullOption(string key)
		{
			OptionMetadata metadata = _argumentMaps.GetFullOption(key);
			metadata.PropertyInfo.SetValue(_runInfo.Value, true);
		}

		private void ResolveShortOption(char key)
		{
			OptionMetadata metadata = _argumentMaps.GetShortOption(key);
			metadata.PropertyInfo.SetValue(_runInfo.Value, true);
		}

		private void ResolveShortCompoundOption(IEnumerable<char> shortKeys)
		{
			foreach(char key in shortKeys)
			{
				OptionMetadata metadata = _argumentMaps.GetShortOption(key);
				metadata.PropertyInfo.SetValue(_runInfo.Value, true);
			}
		}
	}
}
