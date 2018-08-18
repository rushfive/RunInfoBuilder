using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public class ArgumentPropertyMapped<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		internal override void Validate(Type parentType, string parentKey)
		{
			if (Property == null)
			{
				throw new ConfigurationException("Property mapping expression must be provided.",
					typeof(ArgumentPropertyMapped<TRunInfo, TProperty>), parentType, parentKey);
			}
		}

		internal override Stage<TRunInfo> ToStage(ProcessContext<TRunInfo> context)
		{
			return new ArgumentPropertyMappedStage<TRunInfo, TProperty>(Property, context);
		}
	}
}
