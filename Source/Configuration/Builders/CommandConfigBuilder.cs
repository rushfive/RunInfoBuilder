namespace R5.RunInfoBuilder.Configuration
{
	public class CommandConfigBuilder
	{
		private bool _singleCommand { get; set; }
		private CommandPositioning _positioning { get; set; }

		internal CommandConfigBuilder()
		{
			_singleCommand = false;
			_positioning = CommandPositioning.Anywhere;
		}

		public CommandConfigBuilder EnforceSingleCommand()
		{
			_singleCommand = true;
			return this;
		}

		public CommandConfigBuilder EnforcePositionedAtFront()
		{
			_positioning = CommandPositioning.Front;
			return this;
		}

		internal CommandConfig Build()
		{
			return new CommandConfig(_singleCommand, _positioning);
		}
	}
}
