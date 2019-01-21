using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The base abstract class Commands and SubCommands derive from.
	/// <remarks>
	/// These Commands can be nested into a tree structure, unlike the DefaultCommand type.
	/// </remarks>
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the command's associated to.</typeparam>
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
