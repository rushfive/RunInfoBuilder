using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public static class ProcessResult
	{
		public static readonly ProcessStageResult Continue = new Continue();
		public static readonly ProcessStageResult End = new End();
		public static ProcessStageResult SkipNext(int count) => new SkipNext(count);
		public static ProcessStageResult SkipToPosition(int position) => new SkipToPosition(position);
		public static ProcessStageResult SkipToToken(string token) => new SkipToToken(token);
	}

	public abstract class ProcessStageResult
	{
		
	}

	public class Continue : ProcessStageResult
	{

	}

	public class End : ProcessStageResult
	{

	}

	public class SkipNext : ProcessStageResult
	{
		internal SkipNext(int count) { }
	}

	public class SkipToPosition : ProcessStageResult
	{
		internal SkipToPosition(int position) { }
	}

	public class SkipToToken : ProcessStageResult
	{
		internal SkipToToken(string token) { }
	}
}
