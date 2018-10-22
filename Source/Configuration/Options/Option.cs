using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace R5.RunInfoBuilder
{
	public class Option<TRunInfo, TProperty> : OptionBase<TRunInfo>
		where TRunInfo : class
	{
		public string HelpToken { get; set; }
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }
		public Func<TProperty, ProcessStageResult> OnParsed { get; set; }

		public Option() 
			: base(typeof(TProperty)) { }

		internal override List<Action<int>> Rules() => ValidationRules.Options.Rules(this);

		internal override OptionProcessInfo<TRunInfo> GetProcessInfo()
		{
			(Action<TRunInfo, object> Setter, Type Type) = OptionSetterFactory<TRunInfo>.CreateSetter(this);

			return new OptionProcessInfo<TRunInfo>(Setter, Type, OnParsed);
		}

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
