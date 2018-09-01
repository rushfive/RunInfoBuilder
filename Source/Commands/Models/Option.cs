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

		internal abstract void Validate();
	}

	public class Option<TRunInfo, TProperty> : OptionBase<TRunInfo>
		where TRunInfo : class
	{
		public string Description { get; set; }
		public string HelpText { get; set; }

		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		//public Type Type => typeof(TProperty);

		public Option() : base(typeof(TProperty)) { }

		internal override void Validate()
		{
			if (Property == null)
			{
				throw new InvalidOperationException("Property mapping expression must be provided.");
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(Property, out string propertyName))
			{
				throw new InvalidOperationException($"Property '{propertyName}' is not writable. Try adding a setter.");
			}
		}
	}
}
