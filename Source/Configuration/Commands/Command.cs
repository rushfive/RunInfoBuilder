using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The command configuration object. All other configurations like arguments start from here.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type that's built from the command.</typeparam>
	public class Command<TRunInfo> : StackableCommand<TRunInfo>
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

		/// <summary>
		/// List of optional global Options associated to the command.
		/// These are scoped to be accessible to any SubCommand in the tree.
		/// </summary>
		public List<OptionBase<TRunInfo>> GlobalOptions { get; set; } = new List<OptionBase<TRunInfo>>();


		internal override List<Action<int>> Rules() => ValidationRules.Commands.Command.Rules(this);
	}
}
