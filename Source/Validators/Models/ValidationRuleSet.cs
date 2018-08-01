using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Validators
{
	internal class ValidationRuleSet<T>
	{
		private List<ValidationRule<T>> _rules { get; }
		private Action<T> _onAnyInvalidCallback { get; }

		internal ValidationRuleSet(
			List<ValidationRule<T>> rules,
			Action<T> onAnyInvalidCallback = null)
		{
			_rules = rules;
			_onAnyInvalidCallback = onAnyInvalidCallback;
		}

		internal ValidationRuleSet(
			ValidationRule<T> rule,
			Action<T> onAnyInvalidCallback = null)
			: this(new List<ValidationRule<T>> { rule }, onAnyInvalidCallback)
		{
		}

		internal void Validate(T validationObject)
		{
			bool allRulesPassed = true;

			foreach (ValidationRule<T> rule in _rules)
			{
				bool passed = rule.Validate(validationObject);
				allRulesPassed = allRulesPassed && passed;
			}

			if (!allRulesPassed && _onAnyInvalidCallback != null)
			{
				_onAnyInvalidCallback(validationObject);
			}
		}
	}
}
