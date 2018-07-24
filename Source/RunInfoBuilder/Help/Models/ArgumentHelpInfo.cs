using System.Reflection;

namespace R5.RunInfoBuilder.Help
{
	public class ArgumentHelpInfo
	{
		public string Key { get; }
		public string Description { get; }
		public PropertyInfo PropertyInfo { get; }
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
