using R5.RunInfoBuilder.Commands;

namespace R5.RunInfoBuilder
{
	// todo: allow advanced config AND hook into DI
	public class RunInfoBuilder
	{
		public ICommandStore Commands { get; }

		public RunInfoBuilder()
		{

		}
	}
	
	
}
