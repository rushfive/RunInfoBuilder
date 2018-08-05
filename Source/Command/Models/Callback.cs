using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class Callback<TRunInfo> where TRunInfo : class
	{
		// use for "SET BY USER" check
		public Func<ProcessContext<TRunInfo>, CallbackResult> Func { get; set; }
		public CallbackTiming Timing { get; set; }
		public CallbackOrder Order { get; set; }
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
