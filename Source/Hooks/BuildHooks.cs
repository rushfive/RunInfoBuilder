using System;

namespace R5.RunInfoBuilder
{
	public class BuildHooks
	{
		private Action<string[]> _onStart { get; set; }
		internal bool OnStartIsSet => _onStart != null;

		public void SetOnStartBuild(Action<string[]> onStartCallback)
		{
			_onStart = onStartCallback ?? 
				throw new ArgumentNullException(nameof(onStartCallback), "Callback must be provided.");
		}

		internal void InvokeOnStart(string[] args)
		{
			_onStart?.Invoke(args);
		}
	}
}
