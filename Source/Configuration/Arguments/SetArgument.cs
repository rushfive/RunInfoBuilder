using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	public class SetArgument<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		public List<(string Label, TProperty Value)> Values { get; set; }
			= new List<(string, TProperty)>();

		internal override List<Action<int>> Rules() => ValidationRules.Arguments.Set.Rules(this);

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
	}
}
