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

		ICommandStore Add<TRunInfo, TProperty>(CommandPropertyMapped<TRunInfo, TProperty> command)
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

		public ICommandStore Add<TRunInfo, TProperty>(CommandPropertyMapped<TRunInfo, TProperty> command) where TRunInfo : class
		{
			throw new NotImplementedException();
		}

		public ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand) where TRunInfo : class
		{
			throw new NotImplementedException();
		}
	}
	
}
