//using R5.RunInfoBuilder.ArgumentParser;
//using R5.RunInfoBuilder.Store;
//using System;
//using System.Reflection;

//namespace R5.RunInfoBuilder.Pipeline
//{
//	internal class ArgumentStage<TRunInfo> : ProcessPipelineStageBase<TRunInfo>
//		where TRunInfo : class
//	{
//		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }
//		private RunInfo<TRunInfo> _runInfo { get; }
//		private IParser _parser { get; }
//		private IArgumentTokenizer _tokenizer { get; }

//		internal ArgumentStage(
//			IArgumentMetadata<TRunInfo> argumentMaps,
//			RunInfo<TRunInfo> runInfo,
//			IParser parser,
//			IArgumentTokenizer tokenizer)
//			: base(ProgramArgumentType.Argument)
//		{
//			_argumentMaps = argumentMaps;
//			_runInfo = runInfo;
//			_parser = parser;
//			_tokenizer = tokenizer;
//		}

//		internal override (int SkipNext, AfterProcessingStage AfterStage) Process(ProcessArgumentContext<TRunInfo> context)
//		{
//			if (_parser == null)
//			{
//				throw new InvalidOperationException("Cannot process stage because parser hasn't been set.");
//			}
			
//			(string argumentKey, string argumentValue) = _tokenizer.TokenizeArgument(context.Token);

//			ArgumentMetadata metadata = _argumentMaps.GetArgument(argumentKey);
//			PropertyInfo propertyInfo = metadata.PropertyInfo;

//			if (!this._parser.TryParseAs(propertyInfo.PropertyType, argumentValue, out object parsed))
//			{
//				throw new RunInfoBuilderException($"Failed to parse argument value '{argumentValue}' as a '{propertyInfo.PropertyType.Name}' type.");
//			}

//			if (metadata.ValidateFunction != null)
//			{
//				bool isValid = metadata.ValidateFunction(parsed);
//				if (!isValid)
//				{
//					throw new RunInfoBuilderException($"Argument value '{argumentValue}' failed to validate.");
//				}
//			}

//			propertyInfo.SetValue(_runInfo.Value, parsed);

//			return (0, AfterProcessingStage.Continue);
//		}
//	}
//}
