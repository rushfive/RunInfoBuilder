namespace OLD.Configuration
{
	internal class ProcessConfig
	{
		internal HandleUnresolvedArgument HandleUnresolved { get; }
		internal bool DuplicateArgumentsAllowed { get; }

		internal ProcessConfig(
			HandleUnresolvedArgument handleUnresolved,
			bool duplicateArgumentsAllowed)
		{
			HandleUnresolved = handleUnresolved;
			DuplicateArgumentsAllowed = duplicateArgumentsAllowed;
		}
	}
}
