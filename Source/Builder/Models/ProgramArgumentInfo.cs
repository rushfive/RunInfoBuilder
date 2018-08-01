namespace R5.RunInfoBuilder
{
    public class ProgramArgument
    {
		public int Position { get; }
		public string ArgumentToken { get; }
		public ProgramArgumentType Type { get; }

		internal ProgramArgument(
			int position,
			string rawArgumentToken,
			ProgramArgumentType type)
		{
			Position = position;
			ArgumentToken = rawArgumentToken;
			Type = type;
		}
	}
}
