using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Configuration.Validators.Rules;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// Defines a 1-to-1 mapping of a program argument to a property on the RunInfo.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the argument's associated to.</typeparam>
	public class PropertyArgument<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		/// <summary>
		/// An expression of the RunInfo property the value will be bound to.
		/// </summary>
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		/// <summary>
		/// An optional callback that's invoked after the program argument is successfully parsed.
		/// </summary>
		/// <remarks>
		/// This is invoked before the parsed value is bound to the RunInfo property.
		/// </remarks>
		public Func<TProperty, ProcessStageResult> OnParsed { get; set; }

		internal override List<Action<int>> Rules() => ValidationRules.Arguments.Property.Rules(this);

		internal override Stage<TRunInfo> ToStage()
		{
			return new PropertyArgumentStage<TRunInfo, TProperty>(Property, OnParsed);
		}

		internal override string GetHelpToken()
		{
			if (!string.IsNullOrWhiteSpace(HelpToken))
			{
				return HelpToken;
			}

			return HelpTokenResolver.ForPropertyArgument<TProperty>();
		}

		internal override void ValidateArg(int commandLevel)
		{
			ArgumentRules.Property.PropertyMappingIsSet(this, commandLevel);
			ArgumentRules.Property.MappedPropertyIsWritable(this, commandLevel);
		}
	}
}
