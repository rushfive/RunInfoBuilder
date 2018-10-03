using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class DefaultCommand<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{


		internal void Validate(int commandLevel)
		{
			throw new NotImplementedException();
		}
	}
}
