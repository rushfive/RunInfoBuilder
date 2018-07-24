namespace R5.RunInfoBuilder
{
    internal class ProgramArgumentInfo
    {
		internal int Position { get; }
		internal string RawArgumentToken { get; }
		internal ProgramArgumentType Type { get; }

		internal ProgramArgumentInfo(
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
