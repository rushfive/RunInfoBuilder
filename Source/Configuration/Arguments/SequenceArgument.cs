using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	public class SequenceArgument<TRunInfo, TListProperty> : ArgumentBase<TRunInfo>
			where TRunInfo : class
	{
		public Expression<Func<TRunInfo, List<TListProperty>>> ListProperty { get; set; }
		
		internal override void Validate(int commandLevel)
		{
			if (ListProperty == null)
			{
				throw new CommandValidationException("SequenceArgument is missing its property mapping expression.",
					CommandValidationError.NullPropertyExpression, commandLevel);
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(ListProperty, out string propertyName))
			{
				throw new CommandValidationException($"SequenceArgument's property '{propertyName}' "
					+ "is not writable. Try adding a setter.",
					CommandValidationError.PropertyNotWritable, commandLevel);
			}
		}

		internal override Stage<TRunInfo> ToStage()
		{
			return new SequenceArgumentStage<TRunInfo, TListProperty>(ListProperty);
		}
	}
}
