namespace R5.RunInfoBuilder.Help
{
	public class CommandHelpInfo
	{
		public string Key { get; }
		public string Description { get; }

		internal CommandHelpInfo(string key, string description)
		{
			Key = key;
			Description = description;
		}
	}
}