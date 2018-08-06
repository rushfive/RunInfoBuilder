using R5.RunInfoBuilder.Command.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command
{
	public interface ICommandStore
	{
		ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		ICommandStore Add<TRunInfo, TProperty>(Command<TRunInfo, TProperty> command)
			where TRunInfo : class;

		ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class;
	}

	internal class CommandStore : ICommandStore
	{
		public ICommandStore Add<TRunInfo>(Command<TRunInfo> command) where TRunInfo : class
		{
			throw new NotImplementedException();
		}

		public ICommandStore Add<TRunInfo, TProperty>(Command<TRunInfo, TProperty> command) where TRunInfo : class
		{
			throw new NotImplementedException();
		}

		public ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand) where TRunInfo : class
		{
			throw new NotImplementedException();
		}
	}
}
