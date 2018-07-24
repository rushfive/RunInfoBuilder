namespace R5.RunInfoBuilder
{
	internal class RunInfo<TRunInfo>
		where TRunInfo : class
    {
		public TRunInfo Value { get; }

		public RunInfo(TRunInfo value)
		{
			Value = value;
		}
    }
}
