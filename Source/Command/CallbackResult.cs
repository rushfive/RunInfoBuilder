using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command
{
	//public static class CallbackResult
	//{
		
	//}

	public abstract class CallbackResult
	{
		public static readonly CallbackResult Continue = new Continue();
		public static readonly CallbackResult KillProcess = new KillProcess();
		public static CallbackResult SkipNext(int count) => new SkipNext(count);
		public static CallbackResult SkipToPosition(int position) => new SkipToPosition(position);
		public static CallbackResult SkipToToken(string token) => new SkipToToken(token);
	}

	public class Continue : CallbackResult
	{

	}

	public class SkipNext : CallbackResult
	{
		internal SkipNext(int count) { }
	}

	public class SkipToPosition : CallbackResult
	{
		internal SkipToPosition(int position) { }
	}

	public class SkipToToken : CallbackResult
	{
		internal SkipToToken(string token) { }
	}

	public class KillProcess : CallbackResult
	{

	}
}
