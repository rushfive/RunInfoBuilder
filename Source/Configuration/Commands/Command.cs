using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	public class Command<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public List<Command<TRunInfo>> SubCommands { get; set; } = new List<Command<TRunInfo>>();

		internal override List<Action<int>> Rules() => ValidationRules.Commands.Command.Rules(this);
	}
}
