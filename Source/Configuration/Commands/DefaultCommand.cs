using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The configuration for the default behavior when the program is run without specifying a command key.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the command's associated to.</typeparam>
	public class DefaultCommand<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		internal Stage<TRunInfo> ToStage()
		{
			return new DefaultCommandStage<TRunInfo>(OnMatched);
		}
	}
}
