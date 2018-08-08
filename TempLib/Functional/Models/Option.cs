using System;
using Unit = System.ValueTuple;

namespace R5.Lib.Functional
{
	using static F;

	public static partial class F
	{
		public static Option<T> Some<T>(T value) => new Option.Some<T>(value);
		public static Option.None None => Option.None.Default;
	}

	public struct Option<T>
	{
		private readonly T _value;
		private readonly bool _isSome;
		private bool _isNone => !_isSome;

		private Option(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
				
			_isSome = true;
			_value = value;
		}

		public static implicit operator Option<T>(Option.None _) => new Option<T>();
		public static implicit operator Option<T>(Option.Some<T> some) => new Option<T>(some.Value);

		public static implicit operator Option<T>(T value)
		   => value == null ? None : Some(value);

		public R Match<R>(Func<R> None, Func<T, R> Some)
			=> _isSome ? Some(_value) : None();
	}

	namespace Option
	{
		public struct None
		{
			public static readonly None Default = new None();
		}

		public struct Some<T>
		{
			public T Value { get; }

			public Some(T value)
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value)
					   , "Cannot wrap a null value in a 'Some'; use 'None' instead");
				}
					
				Value = value;
			}
		}
	}
}
