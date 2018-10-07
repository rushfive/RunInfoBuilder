using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Samples.HelpExamples.Commands
{
	public class CheckoutRunInfo
	{
		// Arguments
		public string Branch { get; set; }

		// Options
		public bool Quiet { get; set; }
		public bool Force { get; set; }
		public bool Track { get; set; }
	}

	public class Checkout
	{
		public static Command<CheckoutRunInfo> Configuration =>
			new Command<CheckoutRunInfo>
			{
				Key = "checkout",
				Description = "Switch branches or restore working tree files.",
				Arguments =
				{
					new PropertyArgument<CheckoutRunInfo, string>
					{
						HelpToken = "<branch>",
						Property = ri => ri.Branch
					}
				},
				Options =
				{
					new Option<CheckoutRunInfo, bool>
					{
						Key = "quiet | q",
						Property = ri => ri.Quiet
					},
					new Option<CheckoutRunInfo, bool>
					{
						Key = "force | f",
						Property = ri => ri.Force
					},
					new Option<CheckoutRunInfo, bool>
					{
						Key = "track | t",
						Property = ri => ri.Track
					}
				}
			};
	}
}
