using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public static class ProcessResult
	{
		public static readonly ProcessNodeResult Continue = new Continue();
		public static readonly ProcessNodeResult End = new End();
		public static ProcessNodeResult SkipNext(int count) => new SkipNext(count);
		public static ProcessNodeResult SkipToPosition(int position) => new SkipToPosition(position);
		public static ProcessNodeResult SkipToToken(string token) => new SkipToToken(token);
	}

	public abstract class ProcessNodeResult
	{
		
	}

	public class Continue : ProcessNodeResult
	{

	}

	public class End : ProcessNodeResult
	{

	}

	public class SkipNext : ProcessNodeResult
	{
		internal SkipNext(int count) { }
	}

	public class SkipToPosition : ProcessNodeResult
	{
		internal SkipToPosition(int position) { }
	}

	public class SkipToToken : ProcessNodeResult
	{
		internal SkipToToken(string token) { }
	}
}
