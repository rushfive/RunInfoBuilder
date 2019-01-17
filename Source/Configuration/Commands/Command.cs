using R5.RunInfoBuilder.Processor.Stages;
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
		/// <summary>
		/// List of optional global Options associated to the command.
		/// These are scoped to be accessible to any SubCommand in the tree.
		/// </summary>
		public List<OptionBase<TRunInfo>> GlobalOptions { get; set; } = new List<OptionBase<TRunInfo>>();

		internal Stage<TRunInfo> ToStage()
		{
			return new CommandStage<TRunInfo>(OnMatched);
		}
	}
}
