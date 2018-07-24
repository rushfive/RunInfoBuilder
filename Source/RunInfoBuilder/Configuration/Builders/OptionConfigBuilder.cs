namespace R5.RunInfoBuilder.Configuration
{
	public class OptionConfigBuilder
	{
		private ArgumentOptionPositioning _positioning { get; set; }

		internal OptionConfigBuilder()
		{
			_positioning = ArgumentOptionPositioning.Anywhere;
		}
		
		public OptionConfigBuilder PositionAnywhere()
		{
			_positioning = ArgumentOptionPositioning.Anywhere;
			return this;
		}

		public OptionConfigBuilder PositionEnforceAfterCommands()
		{
			_positioning = ArgumentOptionPositioning.AfterCommands;
			return this;
		}

		internal OptionConfig Build()
		{
			return new OptionConfig(_positioning);
		}
	}
}
