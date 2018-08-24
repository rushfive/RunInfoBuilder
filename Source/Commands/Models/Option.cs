using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public interface IOption
	{
		string Key { get; }
		Type Type { get; }
	}

	public class Option<TRunInfo, TProperty> : IOption
		where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string HelpText { get; set; }

		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }

		public Type Type => typeof(TProperty);

		internal void Validate(Type parentType, string parentKey)
		{
			var type = typeof(Option<TRunInfo, TProperty>);

			if (string.IsNullOrWhiteSpace(Key))
			{
				throw new ConfigurationException("Key must be provided.",
					type, parentType, parentKey);
			}

			if (Property == null)
			{
				throw new ConfigurationException("Property must be provided.",
					type, parentType, parentKey);
			}
		}
	}
}
