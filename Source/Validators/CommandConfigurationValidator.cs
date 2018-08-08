using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Validators
{
	internal interface ICommandConfigurationValidator<TRunInfo> where TRunInfo : class
	{
		void Validate(Command<TRunInfo> command);

		void Validate<TProperty>(CommandPropertyMapped<TRunInfo, TProperty> command);

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

		public void Validate<TProperty>(CommandPropertyMapped<TRunInfo, TProperty> command)
		{
			throw new NotImplementedException();
		}

		public void Validate(DefaultCommand<TRunInfo> defaultCommand)
		{
			throw new NotImplementedException();
		}
	}
}
