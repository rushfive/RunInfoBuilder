using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public abstract class OptionBase<TRunInfo>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string HelpText { get; set; }
		public Callback<TRunInfo> Callback { get; } = new Callback<TRunInfo>();

		internal abstract void Validate(Type parentType, string parentKey);

		protected void ValidateBase(Type derivedType, Type parentType, string parentKey)
		{
			if (string.IsNullOrWhiteSpace(Key))
			{
				throw new ConfigurationException("Key must be provided.",
					derivedType, parentType, parentKey);
			}
		}
	}
}
