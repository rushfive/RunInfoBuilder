using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
    public class DefaultCommand<TRunInfo> where TRunInfo : class
    {
		public string Description { get; set; }
		public string Usage { get; set; }
		public Callback<TRunInfo> Callback { get; set; } = new Callback<TRunInfo>();
		public List<ArgumentBase> Arguments { get; set; } = new List<ArgumentBase>();
		public List<Option<TRunInfo>> Options { get; set; } = new List<Option<TRunInfo>>();
	}
}
