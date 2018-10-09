using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Configuration
{
	public abstract class CoreConfigurable
	{
		internal abstract List<Action<int>> Rules();

		internal void Validate(int commandLevel)
		{
			Rules().ForEach(r => r.Invoke(commandLevel));
		}
	}
}
