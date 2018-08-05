using System.Reflection;

namespace OLD.Help
{
	public class CommandHelpInfo : HelpInfo
	{
		internal CommandHelpInfo(string key, string description,
			PropertyInfo propertyInfo)
		{
			Key = key;
			Description = description;
			PropertyInfo = propertyInfo;
		}
	}
}