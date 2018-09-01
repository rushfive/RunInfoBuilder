using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public class PropertyArgument<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		internal override void Validate()
		{
			if (Property == null)
			{
				throw new InvalidOperationException("Property mapping expression must be provided.");
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(Property, out string propertyName))
			{
				throw new InvalidOperationException($"Property '{propertyName}' is not writable. Try adding a setter.");
			}
		}

		internal override Stage<TRunInfo> ToStage(IArgumentParser parser)
		{
			return new PropertyArgumentStage<TRunInfo, TProperty>(parser, Property);
		}
	}
}
