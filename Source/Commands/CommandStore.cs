using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Stages;
using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
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

	internal interface ICommandStoreInternal : ICommandStore
	{
		bool TryGetCommand<TRunInfo>(string key, out Command<TRunInfo> command)
			where TRunInfo : class;

		bool TryGetDefaultCommand<TRunInfo>(out DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class;

		bool IsCommand(string key);
	}

	internal class CommandStore : ICommandStore, ICommandStoreInternal
	{
		private ICommandValidator _validator { get; }
		private IRestrictedKeyValidator _keyValidator { get; }
		private IStagesFactory _stagesFactory { get; }
		

		// new setup
		internal const string DefaultKey = "__DEFAULT__";
		private Dictionary<string, object> _pipelineFactoryMap { get; }



		public CommandStore(
			ICommandValidator validator,
			IRestrictedKeyValidator keyValidator,
			IStagesFactory stagesFactory)
		{
			_validator = validator;
			_keyValidator = keyValidator;
			_stagesFactory = stagesFactory;
			
			//


			// Key: Command key (or DefaultKey)
			// Value: Func<string[], Pipeline<TRunInfo>>
			//        pass args[] to get the corresponding pipeline
			_pipelineFactoryMap = new Dictionary<string, object>();
		}

		// this ADD method shuold create the pipeline HERE, since it has the generic truninfo
		// param ref. create the pipeline OR a callback to get it and save it as an object.
		// when the ruinfobuilder starts building, parse args and find the correct pipeline from this
		// store. ref it as "dynamic" in runinfobuilder and call process!


		public ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
		{
			_validator.Validate(command);

			if (IsCommand(command.Key))
			{
				throw new InvalidOperationException($"Command with key '{command.Key}' has already been configured.");
			}

			Func<string[], Pipeline<TRunInfo>> pipelineFactory = args =>
			{
				Queue<Stage<TRunInfo>> stages = _stagesFactory.Create<TRunInfo>(command);
				return new Pipeline<TRunInfo>(stages, args);
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
				return new Pipeline<TRunInfo>(stages, args);
			};

			_pipelineFactoryMap.Add(CommandStore.DefaultKey, pipelineFactory);
			

			return this;
		}

		public bool TryGetCommandPipeline(string key, out object pipeline)
		{
			pipeline = null;

			if (!_pipelineFactoryMap.ContainsKey(key))
			{
				return false;
			}

			dynamic factory = _pipelineFactoryMap[key];
			pipeline = factory.Invoke();
			return true;
		}

		public bool TryGetCommand<TRunInfo>(string key, out Command<TRunInfo> command)
			where TRunInfo : class
		{
			command = null;

			if (!_commandMap.ContainsKey(key))
			{
				return false;
			}

			command = _commandMap[key] as Command<TRunInfo>;
			return true;
		}

		public bool TryGetDefaultCommand<TRunInfo>(out DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			defaultCommand = _defaultCommand as DefaultCommand<TRunInfo>;
			return _defaultCommand != null;
		}

		public bool IsCommand(string key) => _pipelineFactoryMap.ContainsKey(key);
	}

}
