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
		public static CallbackResult Skip(int count) => new Skip(count);
	}

	public class Continue : CallbackResult
	{

	}

	public class Skip : CallbackResult
	{
		internal Skip(int count) { }
	}

	public class KillProcess : CallbackResult
	{

	}
}
