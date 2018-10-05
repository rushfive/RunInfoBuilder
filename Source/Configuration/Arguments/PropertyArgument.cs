using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class PropertyArgument<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		internal override void Validate(int commandLevel)
		{
			if (Property == null)
			{
				throw new CommandValidationException("PropertyArgument is missing its property mapping expression.",
					CommandValidationError.NullPropertyExpression, commandLevel);
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(Property, out string propertyName))
			{
				throw new CommandValidationException($"PropertyArgument's property '{propertyName}' "
					+ "is not writable. Try adding a setter.",
					CommandValidationError.PropertyNotWritable, commandLevel);
			}
		}

		internal override Stage<TRunInfo> ToStage()
		{
			return new PropertyArgumentStage<TRunInfo, TProperty>(Property);
		}

		internal override string GetHelpToken()
		{
			if (!string.IsNullOrWhiteSpace(HelpToken))
			{
				return HelpToken;
			}

			return HelpTokenResolver.ForPropertyArgument<TProperty>();
		}
	}
}
