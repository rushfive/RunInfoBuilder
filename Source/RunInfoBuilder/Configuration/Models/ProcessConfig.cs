namespace R5.RunInfoBuilder.Configuration
{
	internal class ProcessConfig
	{
		internal HandleUnresolvedArgument HandleUnresolvedArgument { get; set; }

		internal ProcessConfig(HandleUnresolvedArgument handleUnresolvedArgument)
		{
			HandleUnresolvedArgument = handleUnresolvedArgument;
		}
	}
}
