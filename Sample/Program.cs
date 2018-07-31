using R5.RunInfoBuilder;
using R5.RunInfoBuilder.Configuration;
using System;

namespace Sample
{
	public class RunInfo
	{
		public bool Bool1 { get; set; }
		public int Int1 { get; set; }
	}
    class Program
    {
        static void Main(string[] args)
        {
			var setup = new BuilderSetup<RunInfo>();
			setup.Parser.HandleBuiltInPrimitives();

			setup.ConfigureHelp(config =>
			{
				config
					.ConfigureFormatter()
					.InvokeCallbackOnValidationFail();
			});

			RunInfoBuilder<RunInfo> builder = setup.Create();
			
			builder.Store
				.AddOption("bool", ri => ri.Bool1, 'b')
				.AddArgument<int>("int", ri => ri.Int1);

			var argss = new string[] { "--bool", "int=55" };

			BuildResult<RunInfo> result = builder.Build(argss);


            Console.WriteLine("Hello World!");
        }
    }
}
