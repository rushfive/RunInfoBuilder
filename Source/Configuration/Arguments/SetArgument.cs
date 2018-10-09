using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	public class SetArgument<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		public List<(string Label, TProperty Value)> Values { get; set; }
			= new List<(string, TProperty)>();

		internal override string GetHelpToken()
		{
			string result = "<";
			result += string.Join("|", Values.Select(v => v.Label));
			return result + ">";
		}

		internal override Stage<TRunInfo> ToStage()
		{
			return new SetArgumentStage<TRunInfo, TProperty>(Property, Values);
		}

		internal override void Validate(int commandLevel)
		{
			if (Property == null)
			{
				throw new CommandValidationException("Set Argument is missing its property mapping expression.",
					CommandValidationError.NullPropertyExpression, commandLevel);
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(Property, out string propertyName))
			{
				throw new CommandValidationException($"Set Argument's property '{propertyName}' "
					+ "is not writable. Try adding a setter.",
					CommandValidationError.PropertyNotWritable, commandLevel);
			}

			if (Values == null)
			{
				throw new CommandValidationException("List of values for the set must be provided.",
					CommandValidationError.NullObject, commandLevel);
			}

			if (Values.Count <= 1)
			{
				throw new CommandValidationException("Set Arguments must contain at least two items.",
					CommandValidationError.InsufficientCount, commandLevel);
			}

			if (Values.Select(v => v.Label).Distinct().Count() != Values.Count)
			{
				throw new CommandValidationException("Set Argument value labels must be unique within a set.",
					CommandValidationError.DuplicateKey, commandLevel);
			}

			if (Values.Select(v => v.Value).Distinct().Count() != Values.Count)
			{
				throw new CommandValidationException("Set Argument values must be unique within a set.",
					CommandValidationError.DuplicateKey, commandLevel);
			}
		}
	}
}
