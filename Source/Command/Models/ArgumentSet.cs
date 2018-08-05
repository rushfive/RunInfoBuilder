using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class ExclusiveArgumentSet<TRunInfo> : ArgumentBase, IEnumerable<ArgumentBase>
		where TRunInfo : class
	{
		private readonly List<ArgumentBase> _arguments = new List<ArgumentBase>();

		public void Add(ArgumentBase argument) => _arguments.Add(argument);

		public IEnumerator<ArgumentBase> GetEnumerator() => _arguments.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
