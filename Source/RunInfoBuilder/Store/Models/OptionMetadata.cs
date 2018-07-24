using System.Reflection;

namespace R5.RunInfoBuilder.Store
{
	internal class OptionMetadata
	{
		internal string FullKey { get; }
		internal char? ShortKey { get; }
		internal string Description { get; }
		internal PropertyInfo PropertyInfo { get; }

		internal OptionMetadata(
			string fullKey,
			char? shortKey,
			string description,
			PropertyInfo propertyInfo)
		{
			FullKey = fullKey;
			ShortKey = shortKey;
			Description = description;
			PropertyInfo = propertyInfo;
		}
	}
}
