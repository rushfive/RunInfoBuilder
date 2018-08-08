using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class ArgumentList<TRunInfo, TListProperty> : ArgumentBase
			where TRunInfo : class
	{
		public Expression<Func<TRunInfo, List<TListProperty>>> List { get; set; }

		internal override void Validate(Type parentType, string parentKey)
		{
			if (List == null)
			{
				throw new ConfigurationException("Property expression for list must be provided.",
					typeof(ArgumentList<TRunInfo, TListProperty>), parentType, parentKey);
			}
		}
	}
}
