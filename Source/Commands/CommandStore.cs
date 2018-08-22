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

	internal interface ICommandStoreInternal
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
		// store as objects because we dont know the generic run info types
		// until runtime
		private Dictionary<string, object> _commandMap { get; }
		private object _defaultCommand { get; set; }

		public CommandStore(
			ICommandValidator validator,
			IRestrictedKeyValidator keyValidator)
		{
			_validator = validator;
			_keyValidator = keyValidator;
			_commandMap = new Dictionary<string, object>();
		}

		public ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
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

		public ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			_validator.Validate(defaultCommand);

			if (_defaultCommand != null)
			{
				throw new InvalidOperationException("Default command has already been configured.");
			}

			_defaultCommand = defaultCommand;

			return this;
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

		public bool IsCommand(string key) => _commandMap.ContainsKey(key);
	}

}
