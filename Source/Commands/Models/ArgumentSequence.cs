using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public class ArgumentSequence<TRunInfo, TListProperty> : ArgumentBase<TRunInfo>
			where TRunInfo : class
	{
		public Expression<Func<TRunInfo, List<TListProperty>>> ListProperty { get; set; }

		internal override void Validate(Type parentType, string parentKey)
		{
			if (ListProperty == null)
			{
				throw new ConfigurationException("Property expression for list must be provided.",
					typeof(ArgumentSequence<TRunInfo, TListProperty>), parentType, parentKey);
			}
		}
	}
}
