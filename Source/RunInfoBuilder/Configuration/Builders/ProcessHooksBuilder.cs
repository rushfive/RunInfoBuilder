using R5.RunInfoBuilder.Pipeline;
using System;

namespace R5.RunInfoBuilder.Configuration
{
	public class ProcessHooksBuilder<TRunInfo>
		where TRunInfo : class
	{
		private Action<PreProcessContext<TRunInfo>> _preProcessCallback { get; set; }
		private Action<PostProcessContext<TRunInfo>> _postProcessCallback { get; set; }
		private Func<ProcessArgumentContext<TRunInfo>, ProcessStageResult> _preArgumentCallback { get; set; }
		private Func<ProcessArgumentContext<TRunInfo>, ProcessStageResult> _postArgumentCallback { get; set; }

		internal ProcessHooksBuilder()
		{
			_preProcessCallback = null;
			_postProcessCallback = null;
			_preArgumentCallback = null;
			_postArgumentCallback = null;
		}

		public ProcessHooksBuilder<TRunInfo> EnablePreProcessing(Action<PreProcessContext<TRunInfo>> callback)
		{
			_preProcessCallback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			return this;
		}

		public ProcessHooksBuilder<TRunInfo> EnablePostProcessing(Action<PostProcessContext<TRunInfo>> callback)
		{
			_postProcessCallback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			return this;
		}

		public ProcessHooksBuilder<TRunInfo> EnablePreArgumentProcessing(Func<ProcessArgumentContext<TRunInfo>, ProcessStageResult> callback)
		{
			_preArgumentCallback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			return this;
		}

		public ProcessHooksBuilder<TRunInfo> EnablePostArgumentProcessing(Func<ProcessArgumentContext<TRunInfo>, ProcessStageResult> callback)
		{
			_postArgumentCallback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			return this;
		}

		internal ProcessHooksConfig<TRunInfo> Build()
		{
			return new ProcessHooksConfig<TRunInfo>(
				_preProcessCallback,
				_postProcessCallback,
				_preArgumentCallback,
				_postArgumentCallback);
		}
	}
}
