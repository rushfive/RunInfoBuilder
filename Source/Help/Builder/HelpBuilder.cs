using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Help
{
	internal interface IHelpBuilder<TRunInfo>
		where TRunInfo : class
	{
		string Build(HelpMetadata metadata);
	}

	internal class HelpBuilder<TRunInfo> : IHelpBuilder<TRunInfo>
			where TRunInfo : class
	{
		private HelpFormatStrategy _formatStrategy { get; set; }

		public HelpBuilder(HelpConfig<TRunInfo> config)
		{
			SetStrategyFrom(config);
		}

		private void SetStrategyFrom(HelpConfig<TRunInfo> config)
		{
			// todo
			_formatStrategy = null;
		}

		public string Build(HelpMetadata metadata)
		{
			throw new NotImplementedException();
		}
	}

	// config

	// builder for this?
	public class FormatOptions
	{
		public string ScreenPadding { get; set; }
		public string Padding { get; set; } // same val used for all configurable padding

		public ProgramDescriptionOptions ProgramDescription { get; set; } = new ProgramDescriptionOptions();
		public SectionHeaderOptions SectionHeader { get; set; } = new SectionHeaderOptions();

		//public ColumnSectionOptions Arguments { get; set; }
		//public ColumnSectionOptions Commands { get; set; }
		//public ColumnSectionOptions Options { get; set; }
	}

	public class ProgramDescriptionOptions
	{
		public bool Display { get; set; } = true;
		public int LineBreaks { get; set; } = 1;
		public int PaddingCount { get; set; } = 0;
	}

	public class SectionHeaderOptions
	{
		public int LineBreaks { get; set; } = 1;
		public int PaddingCount { get; set; } = 0;
	}

	public class ArgumentSectionOptions : SectionOptions<ArgumentHelpInfo>
	{

	}

	public class CommandSectionOptions : SectionOptions<CommandHelpInfo>
	{

	}

	public class OptionSectionOptions : SectionOptions<OptionHelpInfo>
	{

	}

	public abstract class SectionOptions<T>
		where T : HelpInfo
	{
		public int LineBreaks { get; set; } = 1;
		public int ItemLineBreaks { get; set; } = 0;
		public int PaddingCount { get; set; } = 1;

		public SingleColumnSectionOptions<T> SingleColumn { get; set; } = null;
		public DoubleColumnSectionOptions<T> DoubleColumn { get; set; } = new DoubleColumnSectionOptions<T>();
	}

	public class SingleColumnSectionOptions<T>
		where T : HelpInfo
	{
		public Func<T, string> ColumnValueFunc { get; set; } = T => "";
	}

	public class DoubleColumnSectionOptions<T>
		where T : HelpInfo
	{
		public int SpacesBetweenColumns { get; set; } = 4;

		public Func<T, string> FirstColumnValueFunc { get; set; }
		public Func<T, string> SecondColumnValueFunc { get; set; }
	}

	


	// strategies

	internal class DefaultFormatStrategy : HelpFormatStrategy
	{
		internal DefaultFormatStrategy()
			: base()
		{

		}

		internal override string Format(HelpMetadata metadata)
		{
			throw new NotImplementedException();
		}
	}

	internal abstract class HelpFormatStrategy
	{
		protected HelpFormatStrategy()
		{
		}

		internal abstract string Format(HelpMetadata metadata);
	}
}
