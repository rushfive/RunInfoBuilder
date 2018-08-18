using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
    internal interface ICallbackElement<TRunInfo> where TRunInfo : class
    {
		Func<CallbackContext<TRunInfo>, ProcessStageResult> Callback { get; }
	}
}
