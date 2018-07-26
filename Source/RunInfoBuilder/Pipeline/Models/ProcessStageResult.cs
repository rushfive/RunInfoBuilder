namespace R5.RunInfoBuilder
{
	public class ProcessStageResult
	{
		internal int SkipArgsCount { get; private set; }
		internal bool ContinueArgumentProcessing { get; private set; }
		internal bool KilledBuildProcess { get; private set; }

		public ProcessStageResult()
		{
			SkipArgsCount = 0;
			ContinueArgumentProcessing = true;
			KilledBuildProcess = false;
		}

		public ProcessStageResult SkipNext(int skip)
		{
			SkipArgsCount = skip;
			return this;
		}

		public ProcessStageResult SkipFurtherProcessingForCurrentArgument()
		{
			ContinueArgumentProcessing = false;
			return this;
		}

		public ProcessStageResult KillBuildProcess()
		{
			KilledBuildProcess = true;
			return this;
		}
	}
}
