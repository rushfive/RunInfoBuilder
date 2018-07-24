using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Validators
{
	internal class NoDuplicateArgumentsRule : ValidationRule<string[]>
	{
		protected override Func<string[], bool> _validateFunction => programArguments =>
		{
			var seen = new HashSet<string>();

			foreach (string argument in programArguments)
			{
				if (seen.Contains(argument))
				{
					return false;
				}
				seen.Add(argument);
			}

			return true;
		};
	}
}
