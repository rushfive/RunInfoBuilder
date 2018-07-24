namespace R5.RunInfoBuilder.Configuration
{
	public class ArgumentConfigBuilder
	{
		private ArgumentOptionPositioning _positioning { get; set; }

		internal ArgumentConfigBuilder()
		{
			_positioning = ArgumentOptionPositioning.Anywhere;
		}

		public ArgumentConfigBuilder PositionAnywhere()
		{
			_positioning = ArgumentOptionPositioning.Anywhere;
			return this;
		}

		public ArgumentConfigBuilder PositionEnforceAfterCommands()
		{
			_positioning = ArgumentOptionPositioning.AfterCommands;
			return this;
		}

		internal ArgumentConfig Build()
		{
			return new ArgumentConfig(_positioning);
		}
	}
}
