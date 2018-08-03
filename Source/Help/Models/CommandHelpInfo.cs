using System.Reflection;

namespace R5.RunInfoBuilder.Help
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