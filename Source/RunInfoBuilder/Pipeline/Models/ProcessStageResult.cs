namespace R5.RunInfoBuilder
{
	internal enum ProcessStageType
	{
		Continue,
		StopCurrent,
		KillBuild
	}

	public class ProcessStageResult
	{
		internal int SkipNextArgumentsCount { get; private set; }
		//internal bool ContinueArgumentProcessing { get; private set; }
		//internal bool KilledBuildProcess { get; private set; }
		internal ProcessStageType Type { get; private set; }

		public ProcessStageResult()
		{
			SkipNextArgumentsCount = 0;
			Type = ProcessStageType.Continue;
			//ContinueArgumentProcessing = true;
			//KilledBuildProcess = false;
		}

		public ProcessStageResult SkipNext(int skip)
		{
			SkipNextArgumentsCount = skip;
			return this;
		}

		public ProcessStageResult StopProcessingCurrentArgument()
		{
			Type = ProcessStageType.StopCurrent;
			return this;
		}

		public ProcessStageResult KillBuildProcess()
		{
			Type = ProcessStageType.KillBuild;
			return this;
		}
	}
}
