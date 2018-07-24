//using R5.RunInfoBuilder.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class KeyValidatorTests
//	{
//		private static IKeyValidator GetValidator()
//		{
//			return new KeyValidator();
//		}

//		public class IsRestrictedKey_StringArg_Method
//		{
//			[Fact]
//			public void NullKeyArgument_Throws()
//			{
//				IKeyValidator validator = GetValidator();
//				Assert.Throws<ArgumentNullException>(() => validator.IsRestrictedKey(null));
//			}

//			[Fact]
//			public void VaildKeyArgument_SuccessfullyAdds()
//			{
//				IKeyValidator validator = GetValidator();

//				Assert.False(validator.IsRestrictedKey("key"));

//				validator.AddRestrictedKey("key");

//				Assert.True(validator.IsRestrictedKey("key"));
//			}
//		}

//		public class IsRestrictedKey_CharArg_Method
//		{
//			[Fact]
//			public void VaildKeyArgument_SuccessfullyAdds()
//			{
//				IKeyValidator validator = GetValidator();

//				Assert.False(validator.IsRestrictedKey('k'));

//				validator.AddRestrictedKey('k');

//				Assert.True(validator.IsRestrictedKey('k'));
//			}
//		}

//		public class AddRestrictedKey_StringArg_Method
//		{
//			[Fact]
//			public void NullKeyArgument_Throws()
//			{
//				IKeyValidator validator = GetValidator();
//				Assert.Throws<ArgumentNullException>(() => validator.AddRestrictedKey(null));
//			}

//			[Fact]
//			public void KeyAlreadyRestricted_Throws()
//			{
//				IKeyValidator validator = GetValidator();
//				validator.AddRestrictedKey("key");
//				Assert.Throws<ArgumentException>(() => validator.AddRestrictedKey("key"));
//			}

//			[Fact]
//			public void ValidKey_SuccessfullyAdds()
//			{
//				IKeyValidator validator = GetValidator();

//				Assert.False(validator.IsRestrictedKey("key"));

//				validator.AddRestrictedKey("key");

//				Assert.True(validator.IsRestrictedKey("key"));
//			}
//		}

//		public class AddRestrictedKey_CharArg_Method
//		{
//			[Fact]
//			public void KeyAlreadyRestricted_Throws()
//			{
//				IKeyValidator validator = GetValidator();

//				validator.AddRestrictedKey('k');

//				Assert.Throws<ArgumentException>(() => validator.AddRestrictedKey('k'));
//			}

//			[Fact]
//			public void ValidKey_SuccessfullyAdds()
//			{
//				IKeyValidator validator = GetValidator();

//				Assert.False(validator.IsRestrictedKey('k'));

//				validator.AddRestrictedKey('k');

//				Assert.True(validator.IsRestrictedKey('k'));
//			}
//		}

//		public class AddRestrictedKeys_StringArgs_Method
//		{
//			[Fact]
//			public void NullKeysArgument_Throws()
//			{
//				IKeyValidator validator = GetValidator();
//				Assert.Throws<ArgumentNullException>(() => validator.AddRestrictedKeys((IEnumerable<string>)null));
//			}

//			[Fact]
//			public void AnyKeys_AlreadyRestricted_Throws()
//			{
//				IKeyValidator validator = GetValidator();

//				validator.AddRestrictedKey("key");

//				var addKeys = new List<string> { "a", "key", "b" };
//				Assert.Throws<ArgumentException>(() => validator.AddRestrictedKeys(addKeys));
//			}

//			[Fact]
//			public void KeysAllValid_SuccessfullyAddsAll()
//			{
//				IKeyValidator validator = GetValidator();

//				var addKeys = new List<string> { "a", "key", "b" };
//				foreach(string key in addKeys)
//				{
//					Assert.False(validator.IsRestrictedKey(key));
//				}

//				validator.AddRestrictedKeys(addKeys);

//				foreach (string key in addKeys)
//				{
//					Assert.True(validator.IsRestrictedKey(key));
//				}
//			}
//		}

//		public class AddRestrictedKeys_CharArgs_Method
//		{
//			[Fact]
//			public void NullKeysArgument_Throws()
//			{
//				IKeyValidator validator = GetValidator();
//				Assert.Throws<ArgumentNullException>(() => validator.AddRestrictedKeys((IEnumerable<char>)null));
//			}

//			[Fact]
//			public void AnyKeys_AlreadyRestricted_Throws()
//			{
//				IKeyValidator validator = GetValidator();

//				validator.AddRestrictedKey('k');

//				var addKeys = new List<char> { 'a', 'k', 'b' };
//				Assert.Throws<ArgumentException>(() => validator.AddRestrictedKeys(addKeys));
//			}

//			[Fact]
//			public void KeysAllValid_SuccessfullyAddsAll()
//			{
//				IKeyValidator validator = GetValidator();

//				var addKeys = new List<char> { 'a', 'k', 'b' };
//				foreach(char key in addKeys)
//				{
//					Assert.False(validator.IsRestrictedKey(key));
//				}

//				validator.AddRestrictedKeys(addKeys);

//				foreach (char key in addKeys)
//				{
//					Assert.True(validator.IsRestrictedKey(key));
//				}
//			}
//		}

//		public class ClearKeys_StringArgs_Method
//		{
//			[Fact]
//			public void NullKeysArgument_Throws()
//			{
//				IKeyValidator validator = GetValidator();
//				Assert.Throws<ArgumentNullException>(() => validator.ClearKeys((IEnumerable<string>)null));
//			}

//			[Fact]
//			public void ValidKeys_SuccessfullyRemoves()
//			{
//				IKeyValidator validator = GetValidator();

//				var keys = new List<string> { "a", "b", "c" };
//				validator.AddRestrictedKeys(keys);

//				foreach(string key in keys)
//				{
//					Assert.True(validator.IsRestrictedKey(key));
//				}

//				validator.ClearKeys(keys);

//				foreach (string key in keys)
//				{
//					Assert.False(validator.IsRestrictedKey(key));
//				}
//			}
//		}

//		public class ClearKeys_CharArgs_Method
//		{
//			[Fact]
//			public void NullKeysArgument_Throws()
//			{
//				IKeyValidator validator = GetValidator();
//				Assert.Throws<ArgumentNullException>(() => validator.ClearKeys((IEnumerable<char>)null));
//			}

//			[Fact]
//			public void ValidKeys_SuccessfullyRemoves()
//			{
//				IKeyValidator validator = GetValidator();

//				var keys = new List<char> { 'a', 'b', 'c'};
//				validator.AddRestrictedKeys(keys);

//				foreach (char key in keys)
//				{
//					Assert.True(validator.IsRestrictedKey(key));
//				}

//				validator.ClearKeys(keys);

//				foreach (char key in keys)
//				{
//					Assert.False(validator.IsRestrictedKey(key));
//				}
//			}
//		}
//	}
//}
