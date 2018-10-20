using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace R5.RunInfoBuilder
{
	public class PropertyArgument<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }
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
	}
}
