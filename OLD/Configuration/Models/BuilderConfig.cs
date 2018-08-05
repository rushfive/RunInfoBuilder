namespace OLD.Configuration
{
	internal class BuilderConfig
	{
		internal bool AlwaysReturnBuildResult { get; }

		internal BuilderConfig(bool alwaysReturnBuildResult)
		{
			AlwaysReturnBuildResult = alwaysReturnBuildResult;
		}
	}
}
