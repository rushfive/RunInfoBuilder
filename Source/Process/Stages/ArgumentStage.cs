using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Store;
using System;
using System.Reflection;

namespace R5.RunInfoBuilder.Process
{
	internal class ArgumentStage<TRunInfo> : StageChain<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentMetadata<TRunInfo> _argumentMetadata { get; }
		private IParser _parser { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private IArgumentTokenizer _tokenizer { get; }

		internal ArgumentStage(
			IArgumentMetadata<TRunInfo> argumentMetadata,
			IParser parser,
			RunInfo<TRunInfo> runInfo,
			IArgumentTokenizer tokenizer)
			: base(handlesType: ProgramArgumentType.Argument)
		{
			_argumentMetadata = argumentMetadata;
			_parser = parser;
			_runInfo = runInfo;
			_tokenizer = tokenizer;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			(string argumentKey, string argumentValue) = _tokenizer.TokenizeArgument(argument.ArgumentToken);

			ArgumentMetadata metadata = _argumentMetadata.GetArgument(argumentKey);
			PropertyInfo propertyInfo = metadata.PropertyInfo;

			if (!this._parser.TryParseAs(propertyInfo.PropertyType, argumentValue, out object parsed))
			{
				throw new RunInfoBuilderException($"Failed to parse argument value '{argumentValue}' as a '{propertyInfo.PropertyType.Name}' type.");
			}

			if (metadata.ValidateFunction != null)
			{
				bool isValid = metadata.ValidateFunction(parsed);
				if (!isValid)
				{
					throw new RunInfoBuilderException($"Argument value '{argumentValue}' failed validation.");
				}
			}

			propertyInfo.SetValue(_runInfo.Value, parsed);

			return GoToNext(argument, contextFactory, validationContext);
		}
	}
}
