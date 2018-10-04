using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder
{
	public interface ICommandStore
	{
		ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class;
	}

	internal class CommandStore : ICommandStore
	{
		internal const string DefaultKey = "__DEFAULT__";
		
		private IStagesFactory _stagesFactory { get; }
		private IArgumentParser _parser { get; }

		// Key: Command key (or DefaultKey)
		// Value: Func<string[], Pipeline<TRunInfo>> (pass args[] to get the corresponding pipeline)
		private Dictionary<string, object> _pipelineFactoryMap { get; }

		// Key: Command key (or DefaultKey)
		// Value: CommandBase<TRunInfo>
		private Dictionary<string, object> _commandMap { get; }

		public CommandStore(
			IStagesFactory stagesFactory,
			IArgumentParser parser)
		{
			_stagesFactory = stagesFactory;
			_parser = parser;

			_pipelineFactoryMap = new Dictionary<string, object>();
			_commandMap = new Dictionary<string, object>();
		}
		
		public ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command), "Command must be provided.");
			}

			if (string.IsNullOrWhiteSpace(command.Key))
			{
				throw new CommandValidationException(
					"Command key must be provided.",
					CommandValidationError.KeyNotProvided, commandLevel: 0);
			}

			if (IsCommand(command.Key))
			{
				throw new InvalidOperationException($"Command with key '{command.Key}' has already been configured.");
			}

			command.Validate(commandLevel: 0);
			
			Func<string[], Pipeline<TRunInfo>> pipelineFactory = args =>
			{
				Queue<Stage<TRunInfo>> stages = _stagesFactory.Create<TRunInfo>(command);

				// skip the first arg (command key)
				args = args.Skip(1).ToArray();

				return new Pipeline<TRunInfo>(stages, args, command, _parser);
			};

			_pipelineFactoryMap.Add(command.Key, pipelineFactory);
			
			return this;
		}

		public ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			if (defaultCommand == null)
			{
				throw new ArgumentNullException(nameof(defaultCommand), "Command must be provided.");
			}

			if (IsCommand(CommandStore.DefaultKey))
			{
				throw new InvalidOperationException("Default command has already been configured.");
			}

			defaultCommand.Validate();

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

				dynamic defaultFactory = _pipelineFactoryMap[CommandStore.DefaultKey];
				return defaultFactory.Invoke(args);
			}

			if (!_pipelineFactoryMap.ContainsKey(args[0]))
			{
				throw new Exception();
			}

			dynamic factory = _pipelineFactoryMap[args[0]];

			return factory.Invoke(args);
		}

		public bool IsCommand(string key) => _pipelineFactoryMap.ContainsKey(key);
	}

}
