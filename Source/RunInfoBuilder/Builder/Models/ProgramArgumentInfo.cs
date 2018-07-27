namespace R5.RunInfoBuilder
{
    internal class ProgramArgument
    {
		internal int Position { get; }
		internal string RawArgumentToken { get; }
		internal ProgramArgumentType Type { get; }

		internal ProgramArgument(
			int position,
			string rawArgumentToken,
			ProgramArgumentType type)
		{
			Position = position;
			RawArgumentToken = rawArgumentToken;
			Type = type;
		}
	}
}
