﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using R5.RunInfoBuilder.Configuration.Validators.Rules;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// Parses and adds program arguments into a specified list on the RunInfo.
	/// </summary>
	/// <remarks>
	/// Continues to consider program arguments until it hits an option, subcommand, or runs out.
	/// </remarks>
	/// <typeparam name="TRunInfo">The RunInfo type the argument's associated to.</typeparam>
	/// <typeparam name="TListProperty">The Type of the list the parsed sequence of argument values are added to.</typeparam>
	public class SequenceArgument<TRunInfo, TListProperty> : ArgumentBase<TRunInfo>
			where TRunInfo : class
	{
		/// <summary>
		/// An expression of the RunInfo list property the parsed values will be added to.
		/// </summary>
		public Expression<Func<TRunInfo, List<TListProperty>>> ListProperty { get; set; }

		/// <summary>
		/// An optional callback that's invoked after each program argument is successfully parsed.
		/// </summary>
		/// <remarks>
		/// This is invoked before the parsed value is added to the list.
		/// </remarks>
		public Func<TListProperty, ProcessStageResult> OnParsed { get; set; }

		/// <summary>
		/// An optional function used to generate the error message on parsing error.
		/// </summary>
		/// <remarks>
		/// The single argument to the Func is the program argument that failed to parse.
		/// </remarks>
		public Func<string, string> OnParseErrorUseMessage { get; set; }

		internal override Stage<TRunInfo> ToStage()
		{
			return new SequenceArgumentStage<TRunInfo, TListProperty>(ListProperty, OnParsed, OnParseErrorUseMessage);
		}

		internal override string GetHelpToken()
		{
			if (!string.IsNullOrWhiteSpace(HelpToken))
			{
				return HelpToken;
			}

			return HelpTokenResolver.ForSequenceArgument<TListProperty>();
		}

		internal override void Validate(int commandLevel)
		{
			ArgumentRules.Sequence.MappedPropertyMustBeSet(this, commandLevel);
			ArgumentRules.Sequence.MappedPropertyIsWritable(this, commandLevel);
		}
	}
}
