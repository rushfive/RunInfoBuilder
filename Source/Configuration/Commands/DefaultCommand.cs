using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	public class DefaultCommand<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		internal override List<Action<int>> Rules() => ValidationRules.Commands.Default.Rules(this);
	}
}
