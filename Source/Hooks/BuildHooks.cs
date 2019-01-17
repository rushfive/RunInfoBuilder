using R5.RunInfoBuilder.Hooks;
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

		private ReturnsWithBase _nullOrEmptyReturns { get; set; }

		/// <summary>
		/// Set the callback that's fired at the very beginning of building.
		/// The program arguments are provided as the single argument to the callback.
		/// </summary>
		/// <param name="onStartCallback">The callback to be invoked.</param>
		public BuildHooks SetOnStartBuild(Action<string[]> onStartCallback)
		{
			_onStart = onStartCallback ?? 
				throw new ArgumentNullException(nameof(onStartCallback), "Callback must be provided.");

			return this;
		}

		public BuildHooks SetNullOrEmptyReturns<TReturn>(Func<TReturn> callback)
		{
			// can probably fix this BY making the callback a generic object/class, instead of just a property
			_nullOrEmptyReturns = callback;
		}

		internal void InvokeOnStartIfSet(string[] args)
		{
			_onStart?.Invoke(args);
		}
	}
}
