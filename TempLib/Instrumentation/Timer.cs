using System;
using System.Diagnostics;
using Unit = System.ValueTuple;
using static System.Console;
using R5.Lib.Functional;

namespace R5.Lib.Instrumentation
{
	public static class Timer
	{
		public static T Time<T>(string operation, Func<T> f)
		{
			var sw = new Stopwatch();
			sw.Start();

			T t = f();

			sw.Stop();

			WriteLine($"Operation '{operation}' took {sw.ElapsedMilliseconds}ms.");
			return t;
		}

		public static void Time(string operation, Action action)
			=> Time<Unit>(operation, action.ToFunc());
	}
}
