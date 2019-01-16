using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	public abstract class StackableCommand<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		/// <summary>
		/// A unique key representing the command.
		/// </summary>
		/// <remarks>
		/// The key only needs to be unique within the same level of command. 
		/// A command and one of its' subcommands can share the same key.
		/// </remarks>
		public string Key { get; set; }

		/// <summary>
		/// List of subcommands that are associated to this command.
		/// </summary>
		public List<SubCommand<TRunInfo>> SubCommands { get; set; } = new List<SubCommand<TRunInfo>>();
	}
}
