using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Process
{
	// notes:
	// preProcess and postProcess should be moved into the builder

	public class ProcessContext<TRunInfo>
		where TRunInfo : class
	{
		public string ArgumentToken { get; }
		public ProgramArgumentType ArgumentType { get; }
		public int Position { get; }
		public string[] ProgramArguments { get; }
		public TRunInfo RunInfo { get; }

		internal ProcessContext(
			ProgramArgument argument,
			List<ProgramArgument> arguments,
			TRunInfo runInfo)
		{
			ArgumentToken = argument.ArgumentToken;
			ArgumentType = argument.Type;
			Position = argument.Position;
			ProgramArguments = arguments.Select(a => a.ArgumentToken).ToArray();
			RunInfo = runInfo;
		}
	}

	internal interface IProcessInvoker
	{
		void Begin(List<ProgramArgument> programArguments);
	}
	internal class ProcessInvoker<TRunInfo> : IProcessInvoker
		where TRunInfo : class
	{
		private Queue<ProgramArgument> _argumentQueue { get; set; }
		private IStageChainFactory<TRunInfo> _chainFactory { get; }
		private RunInfo<TRunInfo> _runInfo { get; }

		public ProcessInvoker(
			IStageChainFactory<TRunInfo> chainFactory,
			RunInfo<TRunInfo> runInfo)
		{
			_chainFactory = chainFactory;
			_runInfo = runInfo;
		}

		public void Begin(List<ProgramArgument> programArguments)
		{
			_argumentQueue = new Queue<ProgramArgument>(programArguments);

			// pass this to every stage for them to create their own contexts on the fly
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory = CreateContextFactory(programArguments, _runInfo.Value);

			try
			{
				while (_argumentQueue.Count > 0)
				{
					ProgramArgument argument = _argumentQueue.Dequeue();

					(StageChainResult result, int skipNext) = _chainFactory
						.Create()
						.Process(argument, contextFactory);
					// the ProcessStage above SHOULD return skip next info too,
					// resulting in queue dequeueing

					while (_argumentQueue.Any() && skipNext-- > 0)
					{
						_argumentQueue.Dequeue();
					}
				}
			}
			catch (Exception ex)
			{
				throw new ProcessException(ex.Message, ex);
			}


			// ALSO: something like checking "unresolved" arguments should be its own stage!
		}

		private static Func<ProgramArgument, ProcessContext<TRunInfo>> CreateContextFactory
			(List<ProgramArgument> arguments, TRunInfo runInfo) =>
				(ProgramArgument argument) => new ProcessContext<TRunInfo>(argument, arguments, runInfo);
	}

	// chain of responsibility pattern
	internal interface IStageChainFactory<TRunInfo>
		where TRunInfo : class
	{
		StageChain<TRunInfo> Create();
	}
	internal class StageChainFactory<TRunInfo> : IStageChainFactory<TRunInfo>
		where TRunInfo : class
	{
		public StageChain<TRunInfo> Create()
		{
			throw new NotImplementedException();
		}
	}

	internal enum StageChainResult
	{
		Continue,
		KillBuild
	}
	internal abstract class StageChain<TRunInfo>
		where TRunInfo : class
	{
		private ProgramArgumentType _handlesType { get; }
		protected StageChain<TRunInfo> _next { get; private set; }

		protected StageChain(ProgramArgumentType handlesType)
		{
			_handlesType = handlesType;
		}

		//internal (StageChainResult Result, int SkipNext) StartProcess(
		//	ProgramArgument argument,
		//	Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory)
		//{

		//}

		internal StageChain<TRunInfo> SetNext(StageChain<TRunInfo> next)
		{
			_next = next;
			return next;
		}

		internal (StageChainResult Result, int SkipNext) TryProcessArgument(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory)
		{
			if (CanProcessArgument(argument))
			{
				return Process(argument, contextFactory);
			}
			return GoToNext(argument, contextFactory);
		}

		protected abstract (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory);

		private bool CanProcessArgument(ProgramArgument argument)
		{
			if (_handlesType == ProgramArgumentType.Unresolved)
			{
				return true;
			}
			return argument.Type == _handlesType;
		}

		protected (StageChainResult Result, int SkipNext) GoToNext(ProgramArgument argument, 
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory)
		{
			if (_next != null)
			{
				return _next.TryProcessArgument(argument, contextFactory);
			}
			return (StageChainResult.Continue, 0);
		}

		protected (StageChainResult Result, int SkipNext) GoToNextFromResult(ProcessStageResult result,
			ProgramArgument argument, Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory)
		{
			switch (result.AfterProcessing)
			{
				case AfterProcessingStage.Continue:
					if (_next != null)
					{
						return _next.TryProcessArgument(argument, contextFactory);
					}
					return (StageChainResult.Continue, result.SkipNextCount);
				case AfterProcessingStage.StopProcessingRemainingStages:
					return (StageChainResult.Continue, result.SkipNextCount);
				case AfterProcessingStage.KillBuild:
					return (StageChainResult.KillBuild, result.SkipNextCount);
				default:
					throw new ArgumentOutOfRangeException(nameof(result.AfterProcessing),
						$"'{result.AfterProcessing}' is nto a valid after processing type.");
			}
		}
	}

	// stages
	// pre process argument (if configured)
	// handle unresolved
	// argument
	// command
	// option
	// post process argument (if configured)

	internal class PreProcessStage<TRunInfo> : StageChain<TRunInfo>
		where TRunInfo : class
	{
		private Func<ProcessContext<TRunInfo>, ProcessStageResult> _callback { get; }

		internal PreProcessStage(Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
			: base(ProgramArgumentType.Unresolved)
		{
			_callback = callback;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory)
		{
			ProcessContext<TRunInfo> context = contextFactory(argument);

			ProcessStageResult result = _callback(context);

			return GoToNextFromResult(result, argument, contextFactory);
		}
	}

	internal class HandleUnresolvedStage<TRunInfo> : StageChain<TRunInfo>
		where TRunInfo : class
	{
		private ProcessConfig _config { get; }

		internal HandleUnresolvedStage(ProcessConfig config)
			: base(ProgramArgumentType.Unresolved)
		{
			_config = config;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory)
		{
			if (argument.Type != ProgramArgumentType.Unresolved)
			{
				return GoToNext(argument, contextFactory);
			}

			switch (_config.HandleUnresolved)
			{
				case HandleUnresolvedArgument.NotAllowed:
					throw new RunInfoBuilderException("Unresolved program arguments are invalid for this configuration.");
				case HandleUnresolvedArgument.AllowedButThrowOnProcess:
					throw new RunInfoBuilderException($"Failed to process program argument '{argument.ArgumentToken}' because it's an unknown type.");
				case HandleUnresolvedArgument.AllowedButSkipOnProcess:
					return (StageChainResult.Continue, 0);
				default:
					throw new ArgumentOutOfRangeException(nameof(_config.HandleUnresolved), 
						$"'{_config.HandleUnresolved}' is not a valid handle unresolved argument type.");
			}
		}
	}

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
			: base(ProgramArgumentType.Argument)
		{
			_argumentMetadata = argumentMetadata;
			_parser = parser;
			_runInfo = runInfo;
			_tokenizer = tokenizer;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory)
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

			return GoToNext(argument, contextFactory);
		}
	}

	internal class CommandStage<TRunInfo> : StageChain<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentMetadata<TRunInfo> _argumentMetadata { get; }
		private RunInfo<TRunInfo> _runInfo { get; }

		internal CommandStage(
			IArgumentMetadata<TRunInfo> argumentMetadata,
			RunInfo<TRunInfo> runInfo)
			: base(ProgramArgumentType.Command)
		{
			_argumentMetadata = argumentMetadata;
			_runInfo = runInfo;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory)
		{
			CommandMetadata<TRunInfo> metadata = _argumentMetadata.GetCommand(argument.ArgumentToken);

			switch (metadata.Type)
			{
				case CommandType.PropertyMapped:
					{
						metadata.PropertyInfo.SetValue(_runInfo.Value, metadata.MappedValue);
						return GoToNext(argument, contextFactory);
					}
					break;
				case CommandType.CustomCallback:
					{
						ProcessContext<TRunInfo> context = contextFactory(argument);

						ProcessStageResult result = metadata.Callback(context);

						return GoToNextFromResult(result, argument, contextFactory);
					}
				case CommandType.MappedAndCallback:
					{
						metadata.PropertyInfo.SetValue(_runInfo.Value, metadata.MappedValue);

						ProcessContext<TRunInfo> context = contextFactory(argument);

						ProcessStageResult result = metadata.Callback(context);

						return GoToNextFromResult(result, argument, contextFactory);
					}
				default:
					throw new ArgumentOutOfRangeException($"'{metadata.Type}' is not a valid command type.");
			}
		}
	}
}
