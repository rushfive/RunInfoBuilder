using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class Argument<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		internal override void Validate(Type parentType, string parentKey)
		{
			// optional mapping to allow for callback processing?

			//if (Property == null)
			//{
			//	throw new ConfigurationException("Property expression must be provided.", 
			//		typeof(Argument<TRunInfo, TProperty>), parentType, parentKey);
			//}
		}
	}
}
