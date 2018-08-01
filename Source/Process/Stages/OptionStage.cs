using R5.RunInfoBuilder.Store;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Process
{
	internal class OptionStage<TRunInfo> : StageChain<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentMetadata<TRunInfo> _argumentMetadata { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private IArgumentTokenizer _tokenizer { get; }

		internal OptionStage(
			IArgumentMetadata<TRunInfo> argumentMetadata,
			RunInfo<TRunInfo> runInfo,
			IArgumentTokenizer tokenizer)
			: base(handlesType: ProgramArgumentType.Option)
		{
			_argumentMetadata = argumentMetadata;
			_runInfo = runInfo;
			_tokenizer = tokenizer;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			(OptionType type, string fullKey, List<char> shortKeys) = _tokenizer.TokenizeOption(argument.ArgumentToken);

			switch (type)
			{
				case OptionType.Full:
					{
						OptionMetadata metadata = _argumentMetadata.GetFullOption(fullKey);
						metadata.PropertyInfo.SetValue(_runInfo.Value, true);
					}
					break;
				case OptionType.Short:
					{
						OptionMetadata metadata = _argumentMetadata.GetShortOption(shortKeys.Single());
						metadata.PropertyInfo.SetValue(_runInfo.Value, true);
					}
					break;
				case OptionType.ShortCompound:
					{
						foreach (char key in shortKeys)
						{
							OptionMetadata metadata = _argumentMetadata.GetShortOption(key);
							metadata.PropertyInfo.SetValue(_runInfo.Value, true);
						}
					}
					break;
				default:
					throw new ArgumentOutOfRangeException($"'{type}' is not a valid option type.");
			}

			return GoToNext(argument, contextFactory, validationContext);
		}
	}
}
