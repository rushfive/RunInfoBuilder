namespace R5.RunInfoBuilder
{
	internal enum AfterProcessingStage
	{
		Continue,
		StopProcessingRemainingStages,
		KillBuild
	}

	public class ProcessStageResult
	{
		internal int SkipNextArgumentsCount { get; private set; }
		internal AfterProcessingStage HandleType { get; private set; }

		public ProcessStageResult()
		{
			SkipNextArgumentsCount = 0;
			HandleType = AfterProcessingStage.Continue;
		}

		public ProcessStageResult SkipNext(int skip)
		{
			SkipNextArgumentsCount = skip;
			return this;
		}

		public ProcessStageResult StopProcessingCurrentArgument()
		{
			HandleType = AfterProcessingStage.StopProcessingRemainingStages;
			return this;
		}

		public ProcessStageResult KillBuildProcess()
		{
			HandleType = AfterProcessingStage.KillBuild;
			return this;
		}
	}
}
