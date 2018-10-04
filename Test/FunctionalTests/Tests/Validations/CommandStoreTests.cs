using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Validations
{
public	class CommandStoreTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class Command
		{
			[Fact]
			public void NullCommand_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add<TestRunInfo>(null);
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.NullObject, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}

			[Theory]
			[InlineData(null)]
			[InlineData("")]
			public void InvalidKey_Throws(string key)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add<TestRunInfo>(new Command<TestRunInfo>
					{
						Key = key
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.KeyNotProvided, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}
			
			[Fact]
			public void DuplicateKey_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add<TestRunInfo>(new Command<TestRunInfo>
					{
						Key = "command"
					});

					builder.Commands.Add<TestRunInfo>(new Command<TestRunInfo>
					{
						Key = "command"
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.DuplicateKey, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}
		}

		public class DefaultCommand
		{
			[Fact]
			public void NullCommand_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.AddDefault<TestRunInfo>(null);
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.NullObject, validationException.ErrorType);
				Assert.Equal(-1, validationException.CommandLevel);
			}
			
			[Fact]
			public void DuplicateKey_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>());
					builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>());
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.DuplicateKey, validationException.ErrorType);
				Assert.Equal(-1, validationException.CommandLevel);
			}
		}
	}
}
