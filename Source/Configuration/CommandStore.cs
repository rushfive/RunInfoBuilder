﻿using R5.RunInfoBuilder.Configuration.Validators;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// Provides methods to store the command configurations.
	/// </summary>
	public class CommandStore
	{
		internal const string DefaultKey = "__DEFAULT__";
		
		private StagesFactory _stagesFactory { get; }
		private ArgumentParser _parser { get; }
		private HelpManager _helpManager { get; }

		// Key: Command key (or DefaultKey)
		// Value: Func<string[], Pipeline<TRunInfo>> (pass args[] to get the corresponding pipeline)
		private Dictionary<string, object> _pipelineFactoryMap { get; }

		// Key: Command key (or DefaultKey)
		// Value: CommandBase<TRunInfo>
		private Dictionary<string, object> _commandMap { get; }
		internal bool AreConfigured => _commandMap.Any();

		internal CommandStore(
			ArgumentParser parser,
			HelpManager helpManager)
		{
			_stagesFactory = new StagesFactory();
			_parser = parser;
			_helpManager = helpManager;

			_pipelineFactoryMap = new Dictionary<string, object>();
			_commandMap = new Dictionary<string, object>();
		}
		
		/// <summary>
		/// Adds a normal command (specified by a unique key).
		/// </summary>
		/// <typeparam name="TRunInfo">The run info Type this command is mapped to.</typeparam>
		/// <param name="command">The command configuration object.</param>
		/// <param name="postBuildCallback">An optional callback that's invoked with the built TRunInfo run info object.</param>
		/// <returns>The CommandStore instance.</returns>
		public CommandStore Add<TRunInfo>(Command<TRunInfo> command, 
			Action<TRunInfo> postBuildCallback = null)
			where TRunInfo : class
		{
			if (command == null)
			{
				throw new CommandValidationException(
					"Command must be provided.",
					CommandValidationError.NullObject, commandLevel: 0);
			}

			if (string.IsNullOrWhiteSpace(command.Key))
			{
				throw new CommandValidationException(
					"Command key must be provided.",
					CommandValidationError.KeyNotProvided, commandLevel: 0);
			}

			if (IsCommand(command.Key))
			{
				throw new CommandValidationException(
					$"Command with key '{command.Key} has already been configured.",
					CommandValidationError.DuplicateKey, commandLevel: 0);
			}
			
			CommandValidator.Validate(command);
			
			Func<string[], Pipeline<TRunInfo>> pipelineFactory = args =>
			{
				Queue<Stage<TRunInfo>> stages = _stagesFactory.Create<TRunInfo>(
					command, command.GlobalOptions.Any(), postBuildCallback);

				// skip the first arg (command key)
				args = args.Skip(1).ToArray();

				return new Pipeline<TRunInfo>(stages, args, command, _parser, command.GlobalOptions);
			};

			_pipelineFactoryMap.Add(command.Key, pipelineFactory);

			_helpManager.ConfigureForCommand(command);
			
			return this;
		}

		/// <summary>
		/// Adds the default command (implied if program arguments don't begin with a configured command key).
		/// </summary>
		/// <typeparam name="TRunInfo">The run info Type this command is mapped to.</typeparam>
		/// <param name="defaultCommand">The default command configuration object.</param>
		/// <param name="postBuildCallback">An optional callback that's invoked with the built TRunInfo run info object.</param>
		/// <returns>The CommandStore instance.</returns>
		public CommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand,
			Action<TRunInfo> postBuildCallback = null)
			where TRunInfo : class
		{
			if (defaultCommand == null)
			{
				throw new CommandValidationException(
					"Command must be provided.",
					CommandValidationError.NullObject, commandLevel: -1);
			}

			if (IsCommand(CommandStore.DefaultKey))
			{
				throw new CommandValidationException(
					"Default command has already been configured.",
					CommandValidationError.DuplicateKey, commandLevel: -1);
			}
			
			DefaultCommandValidator.Validate(defaultCommand);

			Func<string[], Pipeline<TRunInfo>> pipelineFactory = args =>
			{
				Queue<Stage<TRunInfo>> stages = _stagesFactory.Create<TRunInfo>(defaultCommand, postBuildCallback);
				return new Pipeline<TRunInfo>(stages, args, defaultCommand, _parser, globalOptions: null);
			};

			_pipelineFactoryMap.Add(CommandStore.DefaultKey, pipelineFactory);

			_helpManager.ConfigureForDefaultCommand(defaultCommand);
			
			return this;
		}

		internal object ResolvePipelineFromArgs(string[] args)
		{
			if (args.Length == 0 || !IsCommand(args[0]))
			{
				if (!_pipelineFactoryMap.ContainsKey(CommandStore.DefaultKey))
				{
					throw new ProcessException("A DefaultCommand is not configured and a Command key wasn't matched.");
				}

				dynamic defaultFactory = _pipelineFactoryMap[CommandStore.DefaultKey];
				return defaultFactory.Invoke(args);
			}

			if (!_pipelineFactoryMap.ContainsKey(args[0]))
			{
				throw new ProcessException($"Failed to process command '{args[0]}', its pipeline could not be found.");
			}

			dynamic factory = _pipelineFactoryMap[args[0]];

			return factory.Invoke(args);
		}

		internal bool IsCommand(string key) => _pipelineFactoryMap.ContainsKey(key);
	}

}
