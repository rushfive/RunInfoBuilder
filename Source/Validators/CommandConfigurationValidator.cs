﻿using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Validators
{
	internal interface ICommandConfigurationValidator
	{
		void Validate<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		void Validate<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class;
	}

	internal class CommandConfigurationValidator : ICommandConfigurationValidator
	{
		private IRestrictedKeyValidator _keyValidator { get; }

		public CommandConfigurationValidator(IRestrictedKeyValidator keyValidator)
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
				throw new ArgumentException($"'{command.Key}' is already configured as a top level key.");
			}

			command.Validate(parentType: null, parentKey: null);
		}

		public void Validate<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			throw new NotImplementedException();
		}
	}
}