using System.Collections.Generic;

namespace OLD
{
	public class ProgramArgumentError
	{
		public string ArgumentToken { get; }
		public int Position { get; }
		public List<string> Errors { get; }

		internal ProgramArgumentError(string argumentToken,
			int position, List<string> errors)
		{
			ArgumentToken = argumentToken;
			Position = position;
			Errors = errors ?? new List<string>();
		}
	}
}
