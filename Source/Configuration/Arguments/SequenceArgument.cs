using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	public class SequenceArgument<TRunInfo, TListProperty> : ArgumentBase<TRunInfo>
			where TRunInfo : class
	{
		public Expression<Func<TRunInfo, List<TListProperty>>> ListProperty { get; set; }
		public Func<TListProperty, ProcessStageResult> OnParsed { get; set; }

		internal override List<Action<int>> Rules() => ValidationRules.Arguments.Sequence.Rules(this);

		internal override Stage<TRunInfo> ToStage()
		{
			return new SequenceArgumentStage<TRunInfo, TListProperty>(ListProperty, OnParsed);
		}

		internal override string GetHelpToken()
		{
			if (!string.IsNullOrWhiteSpace(HelpToken))
			{
				return HelpToken;
			}

			return HelpTokenResolver.ForSequenceArgument<TListProperty>();
		}
	}
}
