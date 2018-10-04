using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class DefaultCommand<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		internal void Validate()
		{
			base.Validate(commandLevel: 0, "Default Command");
		}
	}
}
