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
			if (_versionManager == null)
			{
				return true;
			}

			if (programArguments.Length > 1 && programArguments.Any(a => _versionManager.IsTrigger(a)))
			{
				return false;
			}

			return true;
		};
	}
}
