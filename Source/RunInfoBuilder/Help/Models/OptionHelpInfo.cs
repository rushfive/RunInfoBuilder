using System.Reflection;

namespace R5.RunInfoBuilder.Help
{
	public class OptionHelpInfo
	{
		public string FullKey { get; }
		public char? ShortKey { get; }
		public string Description { get; }
		public PropertyInfo PropertyInfo { get; }

		internal OptionHelpInfo(
			string fullKey,
			char? shortKey,
			string description,
			PropertyInfo propertyInfo)
		{
			FullKey = fullKey;
			ShortKey = shortKey;
			Description = description;
			PropertyInfo = propertyInfo;
		}
	}
}
