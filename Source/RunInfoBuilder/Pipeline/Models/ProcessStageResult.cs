namespace R5.RunInfoBuilder.Pipeline
{
	public class ProcessStageResult
	{
		public int SkipArgsCount { get; private set; }
		public bool ContinueArgumentProcessing { get; private set; }
		public bool KilledBuildProcess { get; private set; }

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

		public ProcessStageResult SkipFurtherArgumentProcessing()
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
