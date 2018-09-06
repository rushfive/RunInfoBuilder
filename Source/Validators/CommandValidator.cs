using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Validators
{
	internal interface ICommandValidator
	{
		void Validate<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		void Validate<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class;
	}

	internal class CommandValidator : ICommandValidator
	{
		private IRestrictedKeyValidator _keyValidator { get; }

		public CommandValidator(IRestrictedKeyValidator keyValidator)
		{
			_keyValidator = keyValidator;
		}

		public void Validate<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command), "Command must be provided.");
			}

			if (_keyValidator.IsRestricted(command.Key))
			{
				throw new CommandValidationException(
					$"'{command.Key}' is already configured as a top level key.",
					CommandValidationError.RestrictedKey, 0);
			}
			
			command.Validate(commandLevel: 0);
		}

		public void Validate<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			return; // tEMP
			throw new NotImplementedException();
		}
	}
}
