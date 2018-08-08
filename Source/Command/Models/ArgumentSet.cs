using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class ExclusiveArgumentSet<TRunInfo> : ArgumentBase, IEnumerable<ArgumentBase>
		where TRunInfo : class
	{
		internal readonly List<ArgumentBase> Arguments = new List<ArgumentBase>();

		public void Add(ArgumentBase argument) => Arguments.Add(argument);

		public IEnumerator<ArgumentBase> GetEnumerator() => Arguments.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		internal override void Validate(Type parentType, string parentKey)
		{
			if (!Arguments.Any())
			{
				throw new ConfigurationException("Set must contain at least one argument.",
					typeof(ExclusiveArgumentSet<TRunInfo>), parentType, parentKey);
			}
		}
	}
}
