namespace OLD.Help
{
	public class HelpCallbackContext<TRunInfo>
			where TRunInfo : class
	{
		public string FormattedText { get; }
		public HelpMetadata Metadata { get; }

		public HelpCallbackContext(string formatted, HelpMetadata metadata)
		{
			this.FormattedText = formatted;
			this.Metadata = metadata;
		}
	}
}
