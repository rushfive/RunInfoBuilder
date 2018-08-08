using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public class PropertyMapping<TRunInfo, TProperty>
			where TRunInfo : class
	{
		// use for "SET BY USER" check
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }
		public TProperty Value { get; set; }
	}
}
