using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
    internal static class ReflectionHelper<TRunInfo>
		where TRunInfo : class
	{
		public static bool PropertyIsWritable<TPropertyType>(Expression<Func<TRunInfo, TPropertyType>> propertyExpression, out string propertyName)
		{
			PropertyInfo propertyInfo = GetPropertyInfoFromExpression(propertyExpression);
			propertyName = propertyInfo.Name;

			return propertyInfo.CanWrite;
		}

		public static PropertyInfo GetPropertyInfoFromExpression<TPropertyType>(Expression<Func<TRunInfo, TPropertyType>> propertyExpression)
		{
			var memberExpression = propertyExpression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException($"Failed to read the property expression body as a '{nameof(MemberExpression)}' type.");
			}

			var propertyInfo = memberExpression.Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException($"Failed to read the member expression as a '{nameof(PropertyInfo)}' type.");
			}

			return propertyInfo;
		}
	}
}
