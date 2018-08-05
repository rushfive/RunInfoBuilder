using OLD.Process;
using System;

namespace OLD.Configuration
{
	public class ProcessHooksBuilder<TRunInfo>
		where TRunInfo : class
	{
		private Action<BuildContext<TRunInfo>> _preBuildCallback { get; set; }
		private Action<BuildContext<TRunInfo>> _postBuildCallback { get; set; }
		private Func<ProcessContext<TRunInfo>, ProcessStageResult> _preProcessStageCallback { get; set; }
		private Func<ProcessContext<TRunInfo>, ProcessStageResult> _postProcessStageCallback { get; set; }

		internal ProcessHooksBuilder()
		{
			_preBuildCallback = null;
			_postBuildCallback = null;
			_preProcessStageCallback = null;
			_postProcessStageCallback = null;
		}

		public ProcessHooksBuilder<TRunInfo> AddPreBuildCallback(Action<BuildContext<TRunInfo>> callback)
		{
			_preBuildCallback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			return this;
		}

		public ProcessHooksBuilder<TRunInfo> AddPostBuildCallback(Action<BuildContext<TRunInfo>> callback)
		{
			_postBuildCallback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			return this;
		}

		public ProcessHooksBuilder<TRunInfo> EnablePreArgumentProcessing(Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
		{
			_preProcessStageCallback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			return this;
		}

		public ProcessHooksBuilder<TRunInfo> EnablePostArgumentProcessing(Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
		{
			_postProcessStageCallback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback must be provided.");
			return this;
		}

		internal HooksConfig<TRunInfo> Build()
		{
			return new HooksConfig<TRunInfo>(
				_preBuildCallback,
				_postBuildCallback,
				_preProcessStageCallback,
				_postProcessStageCallback);
		}
	}
}
