using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.Lib.Abstractions.Pipeline
{
	public abstract class ProcessStageResult { }

	public class ContinueResult : ProcessStageResult { }

	public class ReturnWithValue<TReturn> : ProcessStageResult
	{
		public TReturn Value { get; }

		public ReturnWithValue(TReturn value)
		{
			this.Value = value;
		}
	}

	// helper to consume and declare results easier
	public static class ProcessResult
	{
		public static readonly ProcessStageResult Continue = new ContinueResult();
		public static ProcessStageResult WithValue<TReturn>(TReturn value) => new ReturnWithValue<TReturn>(value);
		public static ProcessStageResult ReturnWithCompletedTask() => new ReturnWithValue<Task>(Task.CompletedTask);
	}
}
