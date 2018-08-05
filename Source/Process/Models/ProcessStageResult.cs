namespace OLD
{
	internal enum AfterProcessingStage
	{
		Continue,
		StopProcessingRemainingStages,
		KillBuild
	}

	public class ProcessStageResult
	{
		internal int SkipNextCount { get; private set; }
		internal AfterProcessingStage AfterProcessing { get; private set; }

		public ProcessStageResult()
		{
			SkipNextCount = 0;
			AfterProcessing = AfterProcessingStage.Continue;
		}

		public ProcessStageResult SkipNext(int skip)
		{
			SkipNextCount = skip;
			return this;
		}

		public ProcessStageResult StopProcessingCurrentArgument()
		{
			AfterProcessing = AfterProcessingStage.StopProcessingRemainingStages;
			return this;
		}

		public ProcessStageResult KillBuildProcess()
		{
			AfterProcessing = AfterProcessingStage.KillBuild;
			return this;
		}
	}
}
