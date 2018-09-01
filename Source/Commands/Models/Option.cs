using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	// ioption because we dont know tproperty until runtime
	//public interface IOption
	//{
	//	string Key { get; }
	//	Type Type { get; }

	//	//void Validate(ValidationContext context);
	//}

	public abstract class OptionBase<TRunInfo> where TRunInfo : class
	{
		public string Key { get; set; }
		internal Type Type { get; }

		protected OptionBase(Type type)
		{
			Type = type;
		}

		internal abstract void Validate(ValidationContext context);
	}

	public class Option<TRunInfo, TProperty> : OptionBase<TRunInfo>
		where TRunInfo : class
	{
		public string Description { get; set; }
		public string HelpText { get; set; }

		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		//public Type Type => typeof(TProperty);

		public Option() : base(typeof(TProperty)) { }

		internal override void Validate(ValidationContext context)
		{
			ValidateKeys(context);

			if (Property == null)
			{
				throw new InvalidOperationException("Property mapping expression must be provided.");
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(Property, out string propertyName))
			{
				throw new InvalidOperationException($"Property '{propertyName}' is not writable. Try adding a setter.");
			}
		}

		private void ValidateKeys(ValidationContext context)
		{
			if (string.IsNullOrWhiteSpace(Key))
			{
				throw new InvalidOperationException("Option key must be provided.");
			}

			if (!OptionTokenizer.IsValidConfiguration(Key))
			{
				throw new InvalidOperationException($"Option key '{Key}' is not formatted correctly.");
			}

			var (fullKey, shortKey) = OptionTokenizer.TokenizeKeyConfiguration(Key);

			if (!context.IsValidFullOption(fullKey))
			{
				throw new InvalidOperationException($"Full option key '{fullKey}' has already been configured.");
			}

			context.MarkFullOptionSeen(fullKey);

			if (!shortKey.HasValue)
			{
				return;
			}

			if (!context.IsValidShortOption(shortKey.Value))
			{
				throw new InvalidOperationException($"Short option key '{shortKey.Value}' has already been configured.");
			}

			context.MarkShortOptionSeen(shortKey.Value);

		}
	}
}
