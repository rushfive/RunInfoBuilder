namespace R5.RunInfoBuilder
{
    internal class ProgramArgument
    {
		internal int Position { get; }
		internal string ArgumentToken { get; }
		internal ProgramArgumentType Type { get; }

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
