using R5.RunInfoBuilder.Configuration.Validators.Rules;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Processor.Stages;
using System;
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

		/// <summary>
		/// An optional function used to generate the error message on parsing error.
		/// </summary>
		/// <remarks>
		/// The single argument to the Func is the program argument that failed to parse.
		/// </remarks>
		public Func<string, string> OnParseErrorUseMessage { get; set; }

		internal override Stage<TRunInfo> ToStage()
		{
			return new PropertyArgumentStage<TRunInfo, TProperty>(Property, OnParsed, OnParseErrorUseMessage);
		}

		internal override string GetHelpToken()
		{
			if (!string.IsNullOrWhiteSpace(HelpToken))
			{
				return HelpToken;
			}

			return HelpTokenResolver.ForPropertyArgument<TProperty>();
		}

		internal override void Validate(int commandLevel)
		{
			ArgumentRules.Property.PropertyMappingIsSet(this, commandLevel);
			ArgumentRules.Property.MappedPropertyIsWritable(this, commandLevel);
		}
	}
}
