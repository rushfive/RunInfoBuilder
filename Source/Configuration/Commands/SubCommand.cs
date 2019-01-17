namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The Command type that represents any nested commands (ie not the root Command)
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the command's associated to.</typeparam>
	public class SubCommand<TRunInfo> : StackableCommand<TRunInfo>
		where TRunInfo : class
	{
	}
}
