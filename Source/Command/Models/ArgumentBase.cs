using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	// add callbacks to these??
	public abstract class ArgumentBase//<TRunInfo> where TRunInfo : class
	{
		internal abstract void Validate(Type parentType, string parentKey);
	}
}
