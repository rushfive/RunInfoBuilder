using System;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// Provides methods to configure custom callbacks as hooks into
	/// various stages of the build process.
	/// </summary>
	public class BuildHooks
	{
		private Action<string[]> _onStart { get; set; }
		internal bool OnStartIsSet => _onStart != null;

		/// <summary>
		/// Set the callback that's fired at the very beginning of building.
		/// The program arguments are provided as the single argument to the callback.
		/// </summary>
		/// <param name="onStartCallback">The callback to be invoked.</param>
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
