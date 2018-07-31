using R5.RunInfoBuilder.Store;
using System;

namespace R5.RunInfoBuilder.Validators
{
	internal class ValidArgumentTypeRule : ValidationRule<ProgramArgumentValidationInfo[]>
	{
		private IArgumentTypeResolver _argumentTypeResolver { get; }

		internal ValidArgumentTypeRule(IArgumentTypeResolver argumentTypeResolver)
		{
			_argumentTypeResolver = argumentTypeResolver;
		}

		protected override Func<ProgramArgumentValidationInfo[], bool> _validateFunction => argumentInfos =>
		{
			bool allValid = true;

			foreach (ProgramArgumentValidationInfo info in argumentInfos)
			{
				if (!_argumentTypeResolver.TryGetArgumentType(info.RawArgumentToken, out ProgramArgumentType type))
				{
					allValid = false;
					info.AddError($"Failed to resolve argument type from token '{info.RawArgumentToken}'.");
				}
			}

			return allValid;
		};
	}
}
