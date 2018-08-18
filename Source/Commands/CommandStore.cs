using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public interface ICommandStore<TRunInfo> where TRunInfo : class
	{
		ICommandStore<TRunInfo> Add(Command<TRunInfo> command);

		ICommandStore<TRunInfo> AddDefault(DefaultCommand<TRunInfo> defaultCommand);
	}

	internal interface ICommandStoreInternal<TRunInfo> : ICommandStore<TRunInfo>
		where TRunInfo : class
	{
		object Get(string key);

		object GetDefault();
	}

	internal class CommandStore<TRunInfo> : ICommandStore<TRunInfo>, ICommandStoreInternal<TRunInfo>
		where TRunInfo : class
	{
		private ICommandValidator _validator { get; }
		private IRestrictedKeyValidator _keyValidator { get; }

		// keep values as object because we dont know their generic types until runtime
		private Dictionary<string, Command<TRunInfo>> _commandMap { get; }
		private object _defaultCommand { get; set; }

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

		public object Get(string key)
		{
			if (!_commandMap.ContainsKey(key))
			{
				throw new InvalidOperationException($"Command with key '{key}' hasn't been configured.");
			}

			return _commandMap[key];
		}

		public object GetDefault()
		{
			if (_defaultCommand == null)
			{
				throw new InvalidOperationException("Default command hasn't been configured.");
			}

			return _defaultCommand;
		}
	}

}
