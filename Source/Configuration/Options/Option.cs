using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace R5.RunInfoBuilder
{
	public class Option<TRunInfo, TProperty> : OptionBase<TRunInfo>
		where TRunInfo : class
	{
		public string Description { get; set; }
		public string HelpToken { get; set; }

		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		public Option() : base(typeof(TProperty)) { }

		internal override List<Action<int>> Rules() => ValidationRules.Options.Rules(this);

		internal override string GetHelpToken()
		{
			if (!string.IsNullOrWhiteSpace(HelpToken))
			{
				return HelpToken;
			}

			(string fullKey, char? shortKey) = OptionTokenizer.TokenizeKeyConfiguration(Key);

			string result = $"[--{fullKey}";

			if (shortKey.HasValue)
			{
				result += $"|-{shortKey.Value}";
			}

			return result + "]";
		}
	}
}
