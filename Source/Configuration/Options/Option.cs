using R5.RunInfoBuilder.Processor;
using System;
using System.Linq.Expressions;

namespace R5.RunInfoBuilder
{
	public class Option<TRunInfo, TProperty> : OptionBase<TRunInfo>
		where TRunInfo : class
	{
		public string Description { get; set; }
		public string HelpText { get; set; }

		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		public Option() : base(typeof(TProperty)) { }

		internal override void Validate(int commandLevel)
		{
			if (Property == null)
			{
				throw new CommandValidationException($"Option '{Key}' is missing its property mapping expression.",
					CommandValidationError.NullPropertyExpression, commandLevel);
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(Property, out string propertyName))
			{
				throw new CommandValidationException($"Option '{Key}'s property '{propertyName}' "
					+ "is not writable. Try adding a setter.",
					CommandValidationError.PropertyNotWritable, commandLevel);
			}
		}
	}
}
