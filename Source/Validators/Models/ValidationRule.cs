using System;
using System.Diagnostics;

namespace R5.RunInfoBuilder.Validators
{
	internal abstract class ValidationRule<T>
	{
		protected abstract Func<T, bool> _validateFunction { get; }

		public bool Validate(T validationObject)
		{
			Debug.Assert(_validateFunction != null);

			return _validateFunction(validationObject);
		}
	}
}
