using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The base abstract class Commands derive from.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the command's associated to.</typeparam>
	public abstract class CommandBase<TRunInfo>
		where TRunInfo : class
	{
		/// <summary>
		/// Description that's displayed in the help menu.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// List of Arguments required to run the command.
		/// </summary>
		public List<ArgumentBase<TRunInfo>> Arguments { get; set; } = new List<ArgumentBase<TRunInfo>>();

		/// <summary>
		/// List of optional Options associated to the command.
		/// </summary>
		public List<OptionBase<TRunInfo>> Options { get; set; } = new List<OptionBase<TRunInfo>>();

		/// <summary>
		/// An optional callback that's invoked immediately after the command is matched and begins processing.
		/// </summary>
		/// <remarks>
		/// This is the first thing processed in a command (eg before arguments, options, etc).
		/// </remarks>
		public Func<TRunInfo, ProcessStageResult> OnMatched { get; set; }
	}
}
