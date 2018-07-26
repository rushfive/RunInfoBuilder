using R5.RunInfoBuilder.Help;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Configuration
{
	public class HelpConfigBuilder<TRunInfo>
		where TRunInfo : class
	{
		private string _programDescription { get; set; }
		private Action<HelpCallbackContext<TRunInfo>> _callback { get; set; }
		private bool _ignoreCase { get; set; }
		private List<string> _triggers { get; set; }
		private bool _invokeOnValidationFail { get; set; }

		private bool IsValid() => _callback != null && _triggers.Any();

		internal HelpConfigBuilder()
		{
			_programDescription = null;
			_callback = null;
			_ignoreCase = false;
			_triggers = new List<string>();
			_invokeOnValidationFail = false;
		}

		public HelpConfigBuilder<TRunInfo> SetProgramDescription(string programDescription)
		{
			if (string.IsNullOrWhiteSpace(programDescription))
			{
				throw new ArgumentNullException(nameof(programDescription), "Program description must be provided.");
			}

			_programDescription = programDescription;
			return this;
		}

		public HelpConfigBuilder<TRunInfo> SetCallback(Action<HelpCallbackContext<TRunInfo>> callback)
		{
			_callback = callback ?? throw new ArgumentNullException(nameof(callback), "Trigger callback must be provided.");
			return this;
		}

		public HelpConfigBuilder<TRunInfo> IgnoreCase()
		{
			_ignoreCase = true;
			return this;
		}

		public HelpConfigBuilder<TRunInfo> SetTriggers(params string[] triggers)
		{
			if (triggers == null || !triggers.Any())
			{
				throw new ArgumentNullException(nameof(triggers), "Triggers must be provided.");
			}

			_triggers.Clear();
			_triggers.AddRange(triggers);
			return this;
		}

		public HelpConfigBuilder<TRunInfo> ConfigureFormatter()
		{
			// TODO:
			throw new NotImplementedException();
		}

		public HelpConfigBuilder<TRunInfo> InvokeCallbackOnValidationFail()
		{
			_invokeOnValidationFail = true;
			return this;
		}

		internal HelpConfig<TRunInfo> Build()
		{
			if (!IsValid())
			{
				throw new InvalidOperationException("Cannot configure help without specifying triggers and a callback.");
			}

			return new HelpConfig<TRunInfo>(
				_programDescription,
				_callback,
				_ignoreCase,
				_triggers,
				_invokeOnValidationFail);
		}
	}
}
