using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{

	public class OptionWithArguments<TRunInfo> : OptionBase<TRunInfo>
		where TRunInfo : class
	{
		public List<ArgumentBase<TRunInfo>> Arguments { get; } = new List<ArgumentBase<TRunInfo>>();
		// if only a single argument, will assume the NEXT token (if delimited by space) is its value
		//    - even if the next value is technically a valid configuerd option
		// will keep readinig next tokens as if they are part of "Arguments" until we reach another keyword,
		// or end of args

		internal override void Validate(Type parentType, string parentKey)
		{
			var type = typeof(OptionWithArguments<TRunInfo>);

			if (!Arguments.Any())
			{
				throw new ConfigurationException("Arguments must contain at least one item.",
					type, parentType, parentKey);
			}

			ValidateBase(type, parentType, parentKey);
		}
	}
}
