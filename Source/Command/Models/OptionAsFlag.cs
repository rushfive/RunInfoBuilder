using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class OptionAsFlag<TRunInfo> : OptionBase<TRunInfo>
			where TRunInfo : class
	{
		public Expression<Func<TRunInfo, bool>> Property { get; set; }

		internal override void Validate(Type parentType, string parentKey)
		{
			var type = typeof(OptionAsFlag<TRunInfo>);

			if (Property == null)
			{
				throw new ConfigurationException("Property must be provided.",
					type, parentType, parentKey);
			}

			ValidateBase(type, parentType, parentKey);
		}
	}
}
