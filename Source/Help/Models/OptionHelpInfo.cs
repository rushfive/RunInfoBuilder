using System.Reflection;

namespace R5.RunInfoBuilder.Help
{
	// todo move
	public abstract class HelpInfo
	{
		public string Key { get; protected set; }
		public string Description { get; protected set; }
		public PropertyInfo PropertyInfo { get; protected set; }
	}

	public class OptionHelpInfo : HelpInfo
	{
		public char? ShortKey { get; }
		
		internal OptionHelpInfo(
			string key,
			char? shortKey,
			string description,
			PropertyInfo propertyInfo)
		{
			Key = key;
			ShortKey = shortKey;
			Description = description;
			PropertyInfo = propertyInfo;
		}
	}
}
