//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace OLD.Help
//{
//	internal interface IHelpBuilder<TRunInfo>
//		where TRunInfo : class
//	{

//	}

//	internal class HelpBuilder<TRunInfo> : IHelpBuilder<TRunInfo>
//			where TRunInfo : class
//	{
//		private HelpFormatStrategy _formateStrategy { get; }

//		public HelpBuilder()
//		{

//		}
//	}

//	internal static class HelpFormatConstants
//	{
//		internal const char Space = ' ';
//		internal static readonly string NewLine = Environment.NewLine;
//		internal const string Tab = "\t";
//		internal const string Empty = "";
//	}

//	internal class PaddingConfig
//	{
//		internal int SpacesCount { get; }
//		internal string Custom { get; }

//		public PaddingConfig(int spacesCount, string custom)
//		{
//			SpacesCount = spacesCount;
//			Custom = custom;
//		}
//	}

//	internal class ProgramDescriptionConfig : HelpFormatConfig
//	{
//		private string _description { get; }
//		private int _lineBreaksCount { get; }

//		protected ProgramDescriptionConfig(PaddingConfig padding,
//			string description, int lineBreaksCount) 
//			: base(padding)
//		{
//		}

//		internal override string Format()
//		{
//			throw new NotImplementedException();
//		}
//	}


//	// todo: make abstract
//	internal abstract class HelpFormatStrategy
//	{
//		internal virtual PaddingConfig Padding { get; } = new PaddingConfig(4, HelpFormatConstants.Empty);

//		// todo: make protected
//		internal HelpFormatStrategy()
//		{
//			_paddingSpaces = new string(HelpFormatConstants.Space, PaddingSpacesLength);
//		}

//		internal string Format(HelpMetadata metadata, string programDescription)
//		{
//			var sb = new StringBuilder();

//			if (DisplayProgramDescription && !string.IsNullOrWhiteSpace(programDescription))
//			{
//				sb.AppendLine(programDescription);
//				sb.AppendLineWithLineBreaks(programDescription, ProgramDescriptionLineBreaks);
//			}

//			return sb.ToString();
//		}
//	}

//	internal class CommandsFormatConfig
//	{
//		internal virtual string CommandSectionLabel { get; } = "Commands:";
//		internal virtual int CommandSectionLabelLineBreaks { get; } = 0;
//		internal virtual int CommandLabelPaddingCount { get; } = 1;

//		internal virtual TextDelimiter DescriptionDelimeter { get; } = TextDelimiter.Tab;
//		internal virtual int DescriptionTabCount { get; } = 1;
//		internal virtual int DescriptionNewLineCount { get; } = 0;
//		internal virtual string DescriptionCustomDelimeter { get; } = string.Empty;
//	}

//	internal enum TextDelimiter
//	{
//		Tab,
//		NewLine,
//		Custom
//	}

//	internal enum PaddingType
//	{
//		Tab,
//		Spaces,
//		Custom
//	}

//	internal abstract class HelpFormatConfig
//	{
//		protected string PaddingSpaces { get; }
//		protected string PaddingCustom { get; }

//		protected HelpFormatConfig(PaddingConfig padding)
//		{
//			PaddingSpaces = HelpFormatConstants.Space.Repeat(padding.SpacesCount);
//			PaddingCustom = padding.Custom;
//		}

//		internal abstract string Format();

//		protected string GetPaddingBy(PaddingType type)
//		{
//			switch (type)
//			{
//				case PaddingType.Tab:
//					return Tab;
//				case PaddingType.Spaces:
//					return PaddingSpaces;
//				case PaddingType.Custom:
//					return PaddingCustom;
//				default:
//					throw new ArgumentOutOfRangeException(nameof(type), $"'{type}' is not a valid padding type.");
//			}
//		}
//	}
	

//	// todo: abstract
//	internal class SectionHeaderConfig : HelpFormatConfig
//	{
//		internal string Label { get; set; }
//		internal PaddingType PaddingType { get; set; }
//		internal int PaddingCount { get; set; }
//		internal int LineBreaksBelow { get; set; }

//		internal SectionHeaderConfig()
//			: base("   ", "") // todo: pass in computed padding val
//		{

//		}

//		internal override string Format()
//		{
//			return new StringBuilder()
//				.AddPadding(GetPaddingBy(PaddingType), PaddingCount)
//				.AppendLine(Label)
//				.ToString();
//		}
//	}




//	internal static class StringBuilderExtensions
//	{
//		private static string NewLine = Environment.NewLine;

//		public static StringBuilder AppendLineWithLineBreaks(this StringBuilder sb,
//			string value, int count)
//		{
//			if (count < 0)
//			{
//				throw new ArgumentException("Line breaks must be a non-negative value.", nameof(count));
//			}

//			sb.AppendLine(value);

//			while (count-- > 0)
//			{
//				sb.Append(NewLine);
//			}

//			return sb;
//		}

//		public static StringBuilder AppendRepeating(this StringBuilder sb,
//			string value, int count)
//		{
//			if (count < 0)
//			{
//				throw new ArgumentException("Repeating count must be a non-negative value.", nameof(count));
//			}
			
//			while (count-- > 0)
//			{
//				sb.Append(value);
//			}

//			return sb;
//		}

//		public static StringBuilder AppendRepeating(this StringBuilder sb,
//			char value, int count)
//		{
//			return sb.AppendRepeating(value.ToString(), count);
//		}

//		public static StringBuilder AddPadding(this StringBuilder sb, string padding, int count)
//		{
//			if (count < 1)
//			{
//				return sb;
//			}

//			return sb.AppendRepeating(padding, count);
//		}

//		public static string RepeatString(string value, int count)
//		{
//			return new StringBuilder().AppendRepeating(value, count).ToString();
//		}
//	}

//	internal static class StringExtensions
//	{
//		public static string Repeat(this string s, int count)
//		{
//			return new StringBuilder().AppendRepeating(s, count).ToString();
//		}

//		public static string Repeat(this char c, int count)
//		{
//			return new StringBuilder().AppendRepeating(c, count).ToString();
//		}
//	}
//}
