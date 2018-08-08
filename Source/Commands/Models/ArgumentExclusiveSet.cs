using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public class ArgumentExclusiveSet<TRunInfo> : ArgumentBase<TRunInfo>, IEnumerable<ArgumentBase<TRunInfo>>
		where TRunInfo : class
	{
		internal readonly List<ArgumentBase<TRunInfo>> Arguments = new List<ArgumentBase<TRunInfo>>();

		public void Add(ArgumentBase<TRunInfo> argument)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(nameof(argument), "Argument must be provided.");
			}

			Arguments.Add(argument);
		}

		public IEnumerator<ArgumentBase<TRunInfo>> GetEnumerator() => Arguments.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		internal override void Validate(Type parentType, string parentKey)
		{
			if (!Arguments.Any())
			{
				throw new ConfigurationException("Set must contain at least one argument.",
					typeof(ArgumentExclusiveSet<TRunInfo>), parentType, parentKey);
			}
		}
	}
}
