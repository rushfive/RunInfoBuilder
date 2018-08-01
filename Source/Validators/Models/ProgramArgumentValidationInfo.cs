using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace R5.RunInfoBuilder.Validators
{
	internal class ProgramArgumentValidationInfo
	{
		public int Position { get; }
		public string RawArgumentToken { get; }
		public ProgramArgumentType Type { get; private set; }
		public List<string> Errors { get; private set; }

		public bool HasError => Errors.Any();

		public ProgramArgumentValidationInfo(int position, string rawArgumentToken)
		{
			Position = position;
			RawArgumentToken = rawArgumentToken;
			Type = ProgramArgumentType.Unresolved;
			Errors = new List<string>();
		}

		public void SetType(ProgramArgumentType type)
		{
			Type = type;
		}

		public void AddError(string error)
		{
			Errors.Add(error);
		}

		public ProgramArgumentError ToError()
		{
			Debug.Assert(HasError);

			return new ProgramArgumentError(RawArgumentToken, Position, Errors);
		}
	}
}
