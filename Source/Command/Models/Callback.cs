using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class Callback<TRunInfo> where TRunInfo : class
	{
		// use for "SET BY USER" check
		public Func<CallbackContext<TRunInfo>, CallbackResult> Func { get; set; }

		internal bool IsConfigured => Func != null;
	}

	public enum CallbackTiming
	{
		Immediately,
		AfterProcessing
	}

	public enum CallbackOrder
	{
		InOrder,
		Parallel
	}
}
