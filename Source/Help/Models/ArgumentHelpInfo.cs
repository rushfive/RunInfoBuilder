using System.Reflection;

namespace R5.RunInfoBuilder.Help
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
