using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class SubCommand<TRunInfo> : StackableCommand<TRunInfo>
		where TRunInfo : class
	{
		///// <summary>
		///// A unique key representing the command.
		///// </summary>
		///// <remarks>
		///// The key only needs to be unique within the same level of command. 
		///// A command and one of its' subcommands can share the same key.
		///// </remarks>
		//public string Key { get; set; }

		///// <summary>
		///// List of subcommands that are associated to this command.
		///// </summary>
		//public List<SubCommand<TRunInfo>> SubCommands { get; set; } = new List<SubCommand<TRunInfo>>();

		internal override List<Action<int>> Rules() => null;// ValidationRules.Commands.SubCommand.Rules(this);

		internal void ValidateSub(int commandLevel, List<OptionBase<TRunInfo>> globalOptions)
		{
			List<Action<int>> rules = ValidationRules.Commands.SubCommand.Rules(this, globalOptions);
			rules.ForEach(r => r.Invoke(commandLevel));
		}
	}
}
