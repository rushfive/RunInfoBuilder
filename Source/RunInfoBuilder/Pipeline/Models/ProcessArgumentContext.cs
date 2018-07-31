//using R5.RunInfoBuilder.Pipeline;

//namespace R5.RunInfoBuilder
//{
//	public class ProcessContext<TRunInfo> : CallbackContext<TRunInfo>
//		where TRunInfo : class
//	{
//		public string Token { get; }
//		public ProgramArgumentType ArgumentType { get; }
//		public int Position { get; }

//		internal ProcessContext(
//			string token, 
//			ProgramArgumentType argumentType,
//			int position,
//			string[] programArguments,
//			TRunInfo runInfo)
//			: base(programArguments, runInfo)
//		{
//			Token = token;
//			ArgumentType = argumentType;
//			Position = position;
//		}
//	}
//}
