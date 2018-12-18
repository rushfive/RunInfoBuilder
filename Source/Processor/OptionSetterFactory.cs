using System;
using System.Reflection;

namespace R5.RunInfoBuilder.Processor
{
	internal static class OptionSetterFactory<TRunInfo>
		where TRunInfo : class
	{
		internal static (Action<TRunInfo, object> Setter, Type Type) CreateSetter(OptionBase<TRunInfo> option)
		{
			dynamic opt = option;
			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(opt.Property);

			Type valueType = propertyInfo.PropertyType;

			Action<TRunInfo, object> setter = (runInfo, value) =>
			{
				bool isNullableEnum = Nullable.GetUnderlyingType(valueType)?.IsEnum == true;

				if (isNullableEnum)
				{
					if (value != null && value.GetType() != Nullable.GetUnderlyingType(valueType))
					{
						throw new InvalidOperationException($"'{value}' is not a valid '{valueType}' type.");
					}
				}
				else
				{
					if (value.GetType() != valueType)
					{
						throw new InvalidOperationException($"'{value}' is not a valid '{valueType}' type.");
					}
				}

				//if (!(isNullableEnum && value == null) && value.GetType() != valueType)
				//{
				//	throw new InvalidOperationException($"'{value}' is not a valid '{valueType}' type.");
				//}

				propertyInfo.SetValue(runInfo, value);
			};

			return (setter, valueType);
		}
	}
}
