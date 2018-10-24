using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// Requires the program argument to be from the specified set.
	/// </summary>
	/// /// <typeparam name="TRunInfo">The RunInfo type the argument's associated to.</typeparam>
	/// <typeparam name="TProperty">The Type of the property the parsed argument value binds to.</typeparam>
	public class SetArgument<TRunInfo, TProperty> : ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		/// <summary>
		/// An expression of the RunInfo property the value will be bound to.
		/// </summary>
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		/// <summary>
		/// List of tuples containing the pairs of keys and values for the set.
		/// </summary>
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
