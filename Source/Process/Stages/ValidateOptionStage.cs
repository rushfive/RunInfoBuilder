using R5.RunInfoBuilder.Configuration;

namespace R5.RunInfoBuilder.Process
{
	internal class ValidateOptionStage<TRunInfo> : ValidationStage<TRunInfo>
			where TRunInfo : class
	{
		internal ValidateOptionStage()
			: base(handlesType: ProgramArgumentType.Option)
		{
		}

		protected override void Validate(ProgramArgument argument, ValidationContext validationContext)
		{
		}

		protected override void MarkSeen(string token, ValidationContext validationContext)
			 => validationContext.MarkOptionSeen(token);
	}
}
