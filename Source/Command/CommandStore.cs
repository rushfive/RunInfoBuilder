using R5.RunInfoBuilder.Command.Models;
using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command
{
	public interface ICommandStore
	{
		ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		ICommandStore Add<TRunInfo, TProperty>(CommandMapped<TRunInfo, TProperty> command)
			where TRunInfo : class;

		ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class;
	}

	internal class CommandStore : ICommandStore
	{
		private IKeyValidator _keyValidator { get; }

		public CommandStore(IKeyValidator keyValidator)
		{
			_keyValidator = keyValidator;
		}

		public ICommandStore Add<TRunInfo>(Command<TRunInfo> command) where TRunInfo : class
		{
			throw new NotImplementedException();
		}

		public ICommandStore Add<TRunInfo, TProperty>(CommandMapped<TRunInfo, TProperty> command) where TRunInfo : class
		{
			throw new NotImplementedException();
		}

		public ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand) where TRunInfo : class
		{
			throw new NotImplementedException();
		}
	}


	// todo move
	internal interface ICommandConfigurationValidator<TRunInfo> where TRunInfo : class
	{
		void Validate(Command<TRunInfo> command);

		void Validate<TProperty>(CommandMapped<TRunInfo, TProperty> command);

		void Validate(DefaultCommand<TRunInfo> defaultCommand);
	}

	internal class CommandConfigurationValidator<TRunInfo> : ICommandConfigurationValidator<TRunInfo>
		where TRunInfo : class
	{
		private IKeyValidator _keyValidator { get; }

		public CommandConfigurationValidator(IKeyValidator keyValidator)
		{
			_keyValidator = keyValidator;
		}

		public void Validate(Command<TRunInfo> command)
		{
			// these are top-level validations only
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command), "Command must be provided.");
			}

			if (_keyValidator.IsRestricted(command.Key))
			{
				throw new ArgumentException($"'{command.Key}' is already configured as a top level key.");
			}

			command.Validate(parentType: null, parentKey: null);

			throw new NotImplementedException();
		}

		public void Validate<TProperty>(CommandMapped<TRunInfo, TProperty> command)
		{
			throw new NotImplementedException();
		}

		public void Validate(DefaultCommand<TRunInfo> defaultCommand)
		{
			throw new NotImplementedException();
		}
	}

	internal interface IArgumentConfigurationValidator<TRunInfo> where TRunInfo : class
	{

	}

	internal class ArgumentConfigurationValidator<TRunInfo> : IArgumentConfigurationValidator<TRunInfo>
		where TRunInfo : class
	{
		public ArgumentConfigurationValidator()
		{

		}

		public void Validate(List<ArgumentBase> arguments, Type parentType, string parentKey)
			=> arguments.ForEach(a => a.Validate(parentType, parentKey));
		
	}
}
