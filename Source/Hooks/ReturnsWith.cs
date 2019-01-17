using System;

namespace R5.RunInfoBuilder.Hooks
{
	internal class ReturnsWith<T> : ReturnsWithBase
	{
		private Func<T> _callback { get; }

		internal ReturnsWith(Func<T> callback)
		{
			_callback = callback ?? throw new Exception();
		}

		internal override object Invoke()
		{
			return _callback();
		}
	}

	internal abstract class ReturnsWithBase
	{
		internal abstract object Invoke();
	}
}
