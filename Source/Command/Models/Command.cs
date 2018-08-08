using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class Command<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		internal override void Validate(Type parentType, string parentKey)
		{
			ValidateBase(typeof(Command<TRunInfo>), parentType, parentKey);
		}
	}
}
