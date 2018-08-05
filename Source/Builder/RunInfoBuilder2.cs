using R5.RunInfoBuilder.Command.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Builder
{
	public class Test
	{
		public enum Command
		{
			Status,
			Diff
		}

		public class GitRunInfo
		{
			public bool Bool { get; set; }
		}

		IArgumentStore _store { get; }

		void Main()
		{
			//_store
			//	.AddCommand(new Command<GitRunInfo, bool>
			//	{
			//		Key = "status",
			//		Description = "Lists all new or modified files to be committed",
			//		Usage = "git status",
			//		Mapping =
			//		{

			//		}
			//	});
		}
	}
	public class RunInfoBuilder2
	{
		public IArgumentStore Store { get; }
	}

	public interface IArgumentStore
	{
		IArgumentStore AddCommand<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		IArgumentStore AddCommand<TRunInfo, TProperty>(Command<TRunInfo, TProperty> command)
			where TRunInfo : class;

		IArgumentStore AddDefault();
	}
	
}
