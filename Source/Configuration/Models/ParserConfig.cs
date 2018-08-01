using R5.RunInfoBuilder.ArgumentParser;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Configuration
{
    internal class ParserConfig
    {
		internal AutoParseEnum AutoParseEnum { get; }
		internal List<(Type, Func<string, (bool, object)>)> ParseTypePredicates { get; }

		internal ParserConfig(
			AutoParseEnum autoParseEnum,
			List<(Type, Func<string, (bool, object)>)> typePredicates)
		{
			AutoParseEnum = autoParseEnum;
			ParseTypePredicates = typePredicates;
		}
	}
}
