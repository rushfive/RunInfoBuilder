using R5.RunInfoBuilder.Version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Validators
{
	internal class VersionOnlyArgumentRule : ValidationRule<string[]>
	{
		private IVersionManager _versionManager { get; }

		internal VersionOnlyArgumentRule(IVersionManager versionManager)
		{
			_versionManager = versionManager;
		}

		protected override Func<string[], bool> _validateFunction => programArguments =>
		{
			if (_versionManager == null || programArguments.Length == 1)
			{
				return true;
			}

			bool hasInvalidTrigger = programArguments.Skip(1).Any(a => _versionManager.IsTrigger(a));
			return !hasInvalidTrigger;
		};
	}
}
