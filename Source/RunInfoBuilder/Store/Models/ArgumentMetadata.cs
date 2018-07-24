using System;
using System.Reflection;

namespace R5.RunInfoBuilder.Store
{
	internal class ArgumentMetadata
	{
		internal string Key { get; }
		internal string Description { get; }
		internal string ValidatorDescription { get; }
		internal Func<object, bool> ValidateFunction { get; }
		internal PropertyInfo PropertyInfo { get; }

		internal ArgumentMetadata(
			string key,
			string description,
			string validatorDescription,
			Func<object, bool> validateFunction,
			PropertyInfo propertyInfo)
		{
			Key = key;
			Description = description;
			ValidatorDescription = validatorDescription;
			ValidateFunction = validateFunction;
			PropertyInfo = propertyInfo;
		}
	}
}
