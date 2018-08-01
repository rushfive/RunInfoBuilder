namespace R5.RunInfoBuilder.Configuration
{
	internal enum CommandPositioning
	{
		Anywhere,
		Front
	}

	internal class CommandConfig
	{
		internal bool EnforceSingleCommand { get; }
		internal CommandPositioning Positioning { get; }

		internal CommandConfig(
			bool enforceSingleCommand,
			CommandPositioning positioning)
		{
			EnforceSingleCommand = enforceSingleCommand;
			Positioning = positioning;
		}
	}
}
