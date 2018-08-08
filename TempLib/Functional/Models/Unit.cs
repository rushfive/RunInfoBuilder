using System;
using Unit = System.ValueTuple;

namespace R5.Lib.Functional
{
	using static F;

	public static partial class F
	{
		public static Unit Unit() => default(Unit);
	}

	public static class ActionExtensions
	{
		public static Func<Unit> ToFunc(this Action action)
			=> () => { action(); return Unit(); };

		public static Func<T, Unit> ToFunc<T>(this Action<T> action)
			=> t => { action(t); return Unit(); };
	}
}
