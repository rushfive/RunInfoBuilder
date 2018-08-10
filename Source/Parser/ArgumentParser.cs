using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Parser
{
	public interface IArgumentParser
	{
		IArgumentParser SetPredicateForType<T>(Func<string, (bool isValid, T parsed)> predicateFunc);

		bool TryParseAs(Type type, string value, out object parsed);

		bool TryParseAs<T>(string value, out T parsed);

		bool HandlesType(Type type);

		bool HandlesType<T>();
	}

	//class ArgumentParser
 //   {
 //   }
}
