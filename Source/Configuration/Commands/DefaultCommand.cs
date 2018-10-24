using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The configuration for the default behavior when the program is run without specifying a command key.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the command's associated to.</typeparam>
	public class DefaultCommand<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		internal override List<Action<int>> Rules() => ValidationRules.Commands.Default.Rules(this);
	}
}
