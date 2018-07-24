using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Store
{
	internal class ArgumentPropertyInfo
	{
		internal PropertyInfo Property { get; }
		internal Func<object, bool> ValidateFunction { get; }

		internal ArgumentPropertyInfo(PropertyInfo property, Func<object, bool> validateFunction)
		{
			Property = property;

			ValidateFunction = (object o) =>
			{
				if (o.GetType() != this.Property.PropertyType)
				{
					return false;
				}
				if (validateFunction == null)
				{
					return true;
				}
				return validateFunction(o);
			};
		}
	}
}
