using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Stages;
using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public interface ICommandStore
	{
		ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class;
	}

	//internal interface ICommandStoreInternal : ICommandStore
	//{
	//	bool TryGetCommandPipeline(string key, out object pipeline);

	//	bool TryGetDefaultPipeline(out object pipeline);
	//	//bool TryGetCommand<TRunInfo>(string key, out Command<TRunInfo> command)
	//	//	where TRunInfo : class;

	//	//bool TryGetDefaultCommand<TRunInfo>(out DefaultCommand<TRunInfo> defaultCommand)
	//	//	where TRunInfo : class;

	//	bool IsCommand(string key);
	//}

	internal class CommandStore : ICommandStore
	{
		internal const string DefaultKey = "__DEFAULT__";

		private ICommandValidator _validator { get; }
		private IRestrictedKeyValidator _keyValidator { get; }
		private IStagesFactory _stagesFactory { get; }
		private IArgumentParser _parser { get; }

		// Key: Command key (or DefaultKey)
		// Value: Func<string[], Pipeline<TRunInfo>> (pass args[] to get the corresponding pipeline)
		private Dictionary<string, object> _pipelineFactoryMap { get; }

		// Key: Command key (or DefaultKey)
		// Value: CommandBase<TRunInfo>
		private Dictionary<string, object> _commandMap { get; }

		public CommandStore(
			ICommandValidator validator,
			IRestrictedKeyValidator keyValidator,
			IStagesFactory stagesFactory,
			IArgumentParser parser)
		{
			_validator = validator;
			_keyValidator = keyValidator;
			_stagesFactory = stagesFactory;
			_parser = parser;

			_pipelineFactoryMap = new Dictionary<string, object>();
			_commandMap = new Dictionary<string, object>();
		}

		// this ADD method shuold create the pipeline HERE, since it has the generic truninfo
		// param ref. create the pipeline OR a callback to get it and save it as an object.
		// when the ruinfobuilder starts building, parse args and find the correct pipeline from this
		// store. ref it as "dynamic" in runinfobuilder and call process!


		public ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command), "Command must be provided.");
			}

			_validator.Validate(command);

			if (IsCommand(command.Key))
			{
				throw new InvalidOperationException($"Command with key '{command.Key}' has already been configured.");
			}

			Func<string[], Pipeline<TRunInfo>> pipelineFactory = args =>
			{
				Queue<Stage<TRunInfo>> stages = _stagesFactory.Create<TRunInfo>(command);

				// skip the first arg (command key)
				args = args.Skip(1).ToArray();

				return new Pipeline<TRunInfo>(stages, args, command, _parser);
			};

			_pipelineFactoryMap.Add(command.Key, pipelineFactory);

			_keyValidator.Add(command.Key);

			return this;
		}

		public ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			_validator.Validate(defaultCommand);

			if (IsCommand(CommandStore.DefaultKey))
			{
				throw new InvalidOperationException("Default command has already been configured.");
			}

			Func<string[], Pipeline<TRunInfo>> pipelineFactory = args =>
			{
				Queue<Stage<TRunInfo>> stages = _stagesFactory.Create<TRunInfo>(defaultCommand);
				return new Pipeline<TRunInfo>(stages, args, defaultCommand, _parser);
			};

			_pipelineFactoryMap.Add(CommandStore.DefaultKey, pipelineFactory);
			
			return this;
		}

		internal object ResolvePipelineFromArgs(string[] args)
		{
			if (args.Length == 0 || !IsCommand(args[0]))
			{
				if (!_pipelineFactoryMap.ContainsKey(CommandStore.DefaultKey))
				{
					throw new Exception();
				}

				dynamic defaultFactory = _pipelineFactoryMap[CommandStore.DefaultKey];//.Invoke(args);
				return defaultFactory.Invoke(args);

				// default
				//if (!TryGetDefaultPipeline(out dynamic defaultPipelineFactory))
				//{
				//	throw new Exception();
				//}

				//return defaultPipelineFactory.Invoke(args);
			}

			if (!_pipelineFactoryMap.ContainsKey(args[0]))
			{
				throw new Exception();
			}

			dynamic factory = _pipelineFactoryMap[args[0]];//.Invoke(args);
			//dynamic pipeline = factory.Invoke(args);
			return factory.Invoke(args);

			//
			//if (!TryGetCommandPipelineFactory(args[0], out dynamic pipelineFactory))
			//{
			//	throw new Exception();
			//}

			//return pipelineFactory.Invoke(args);
		}

		//private bool TryGetCommandPipelineFactory(string key, out object factory)
		//{
		//	factory = null;

		//	if (!_pipelineFactoryMap.ContainsKey(key))
		//	{
		//		return false;
		//	}

		//	object factory = _pipelineFactoryMap[key];
		//	//pipeline = factory.Invoke();
		//	return true;
		//}

		//private bool TryGetDefaultPipeline(out object pipeline)
		//{
		//	pipeline = null;

		//	if (!_pipelineFactoryMap.ContainsKey(CommandStore.DefaultKey))
		//	{
		//		return false;
		//	}

		//	dynamic factory = _pipelineFactoryMap[CommandStore.DefaultKey];
		//	pipeline = factory.Invoke();
		//	return true;
		//}

		//internal object GetCommand(string key) => _commandMap[key];

		public bool IsCommand(string key) => _pipelineFactoryMap.ContainsKey(key);
	}

}
