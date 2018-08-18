using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	public static class ProcessResult
	{
		public static readonly ProcessStageResult Continue = new Continue();
		public static readonly ProcessStageResult End = new End();
		public static ProcessStageResult SkipNext(int count) => new SkipNext(count);
		public static ProcessStageResult SkipToPosition(int position) => new SkipToPosition(position);
		public static ProcessStageResult SkipToToken(string token) => new SkipToToken(token);
		public static readonly ProcessStageResult SkipCurrentAfterCallback = new SkipCurrentAfterCallback();
		public static readonly ProcessStageResult SkipToFirstOption = new SkipToFirstOption();
		public static readonly ProcessStageResult SkipToSubCommand = new SkipToSubCommand();
	}

	public abstract class ProcessStageResult
	{
		
	}

	internal class Continue : ProcessStageResult
	{

	}

	internal class End : ProcessStageResult
	{

	}

	internal class SkipNext : ProcessStageResult
	{
		internal SkipNext(int count) { }
	}

	internal class SkipToPosition : ProcessStageResult
	{
		internal SkipToPosition(int position) { }
	}

	internal class SkipToToken : ProcessStageResult
	{
		internal SkipToToken(string token) { }
	}

	internal class SkipCurrentAfterCallback : ProcessStageResult
	{
		internal SkipCurrentAfterCallback() { }
	}

	// do we need?
	internal class SkipToFirstOption : ProcessStageResult
	{

	}

	// do we need?
	internal class SkipToSubCommand : ProcessStageResult
	{

	}
}
