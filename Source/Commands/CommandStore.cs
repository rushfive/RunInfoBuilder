using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public interface ICommandStore<TRunInfo> 
		where TRunInfo : class
	{
		ICommandStore<TRunInfo> Add(Command<TRunInfo> command);

		ICommandStore<TRunInfo> AddDefault(DefaultCommand<TRunInfo> defaultCommand);
	}

	internal interface ICommandStoreInternal<TRunInfo>
		where TRunInfo : class
	{
		bool TryGetCommand(string key, out Command<TRunInfo> command);

		bool TryGetDefaultCommand(out DefaultCommand<TRunInfo> defaultCommand);

		bool IsCommand(string key);
	}

	internal class CommandStore<TRunInfo> : ICommandStore<TRunInfo>, ICommandStoreInternal<TRunInfo>
		where TRunInfo : class
	{
		private ICommandValidator _validator { get; }
		private IRestrictedKeyValidator _keyValidator { get; }
		private Dictionary<string, Command<TRunInfo>> _commandMap { get; }
		private DefaultCommand<TRunInfo> _defaultCommand { get; set; }

		public CommandStore(
			ICommandValidator validator,
			IRestrictedKeyValidator keyValidator)
		{
			_validator = validator;
			_keyValidator = keyValidator;
			_commandMap = new Dictionary<string, Command<TRunInfo>>();
		}

		public ICommandStore<TRunInfo> Add(Command<TRunInfo> command)
		{
			_validator.Validate(command);

			if (_commandMap.ContainsKey(command.Key))
			{
				throw new InvalidOperationException($"Command with key '{command.Key}' has already been configured.");
			}

			_commandMap.Add(command.Key, command);

			_keyValidator.Add(command.Key);

			return this;
		}

		public ICommandStore<TRunInfo> AddDefault(DefaultCommand<TRunInfo> defaultCommand)
		{
			_validator.Validate(defaultCommand);

			if (_defaultCommand != null)
			{
				throw new InvalidOperationException("Default command has already been configured.");
			}

			_defaultCommand = defaultCommand;

			return this;
		}

		public bool TryGetCommand(string key, out Command<TRunInfo> command)
		{
			command = null;

			if (!_commandMap.ContainsKey(key))
			{
				return false;
			}

			command = _commandMap[key];
			return true;
		}

		public bool TryGetDefaultCommand(out DefaultCommand<TRunInfo> defaultCommand)
		{
			defaultCommand = _defaultCommand;
			return _defaultCommand != null;
		}

		public bool IsCommand(string key) => _commandMap.ContainsKey(key);
	}

}
