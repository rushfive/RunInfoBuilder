namespace R5.RunInfoBuilder.Configuration
{
	internal class ProcessConfig
	{
		internal HandleUnresolvedArgument HandleUnresolved { get; set; }

		internal ProcessConfig(HandleUnresolvedArgument handleUnresolved)
		{
			HandleUnresolved = handleUnresolved;
		}
	}
}
