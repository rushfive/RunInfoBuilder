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
		internal Action<string[]> OnStart { get; private set; }
		internal ReturnsWithBase NullOrEmptyReturns { get; private set; }

		/// <summary>
		/// Set the callback that's fired at the very beginning of building.
		/// The program arguments are provided as the single argument to the callback.
		/// </summary>
		/// <param name="callback">The callback to be invoked.</param>
		public BuildHooks OnStartBuild(Action<string[]> callback)
		{
			OnStart = callback ?? 
				throw new ArgumentNullException(nameof(callback), "Callback must be provided.");

			return this;
		}

		/// <summary>
		/// Set the callback that's fired if program arguments is null or empty.
		/// The builder will return the object returned from the callback.
		/// </summary>
		/// <param name="callback">The callback to be invoked.</param>
		public BuildHooks ArgsNullOrEmptyReturns<TReturn>(Func<TReturn> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			}

			NullOrEmptyReturns = new ReturnsWith<TReturn>(callback);
			return this;
		}
	}
}
