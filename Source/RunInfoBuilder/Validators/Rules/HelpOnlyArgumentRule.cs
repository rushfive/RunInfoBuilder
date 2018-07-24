using R5.RunInfoBuilder.Help;
using System;
using System.Linq;

namespace R5.RunInfoBuilder.Validators
{
	internal class HelpOnlyArgumentRule<TRunInfo> : ValidationRule<string[]>
		where TRunInfo : class
	{
		private IHelpManager<TRunInfo> _helpManager { get; }

		public HelpOnlyArgumentRule(IHelpManager<TRunInfo> helpManager)
		{
			_helpManager = helpManager;
		}

		protected override Func<string[], bool> _validateFunction => programArguments =>
		{
			if (_helpManager == null || programArguments.Length == 1)
			{
				return true;
			}

			bool hasInvalidTrigger = programArguments.Skip(1).Any(a => _helpManager.IsTrigger(a));
			return !hasInvalidTrigger;
		};
	}
}
