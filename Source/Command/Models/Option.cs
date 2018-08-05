using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class Option<TRunInfo>
			where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string Usage { get; set; }
		public List<ArgumentBase> Arguments { get; set; }
		// if only a single argument, will assume the NEXT token (if delimited by space) is its value
		//    - even if the next value is technically a valid configuerd option
	}
}
