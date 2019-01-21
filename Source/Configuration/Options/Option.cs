using R5.RunInfoBuilder.Configuration.Validators.Rules;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Linq.Expressions;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The option configuration object.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the option binds a value to.</typeparam>
	/// <typeparam name="TProperty">The type of the RunInfo property the option binds to.</typeparam>
	public class Option<TRunInfo, TProperty> : OptionBase<TRunInfo>
		where TRunInfo : class
	{
		/// <summary>
		/// The token displayed in the help menu representing the option.
		/// </summary>
		public string HelpToken { get; set; }

		/// <summary>
		/// An expression of the RunInfo property the option binds to.
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
		/// The single argument to the Func is the option value that failed to parse.
		/// </remarks>
		public Func<string, string> OnParseErrorUseMessage { get; set; }

		public Option() 
			: base(typeof(TProperty)) { }

		internal override void ValidateOption(int commandLevel)
		{
			OptionRules.PropertyMappingIsSet(this, commandLevel);
			OptionRules.MappedPropertyIsWritable(this, commandLevel);
			OptionRules.OnProcessCallbackNotAllowedForBoolOptions(this, commandLevel);
		}

		internal override OptionProcessInfo<TRunInfo> GetProcessInfo()
		{
			(Action<TRunInfo, object> Setter, Type Type) = OptionSetterFactory<TRunInfo>.CreateSetter(this);

			return new OptionProcessInfo<TRunInfo>(Setter, Type, OnParsed, OnParseErrorUseMessage);
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
