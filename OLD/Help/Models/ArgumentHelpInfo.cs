using System.Reflection;

namespace OLD.Help
{
	public class ArgumentHelpInfo : HelpInfo
	{
		public string ValidatorDescription { get; }

		internal ArgumentHelpInfo(
			string key,
			string description,
			PropertyInfo propertyInfo,
			string validatorDescription)
		{
			Key = key;
			Description = description;
			PropertyInfo = propertyInfo;
			ValidatorDescription = validatorDescription;
		}
	}
}
