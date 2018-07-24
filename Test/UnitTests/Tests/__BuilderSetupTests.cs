//using R5.RunInfoBuilder.Abstractions;
//using R5.RunInfoBuilder.Models;
//using R5.RunInfoBuilder.ProcessPipeline;
//using R5.RunInfoBuilder.Tests.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class BuilderSetupTests
//	{
//		public class Initialization
//		{
//			abstract class BaseClass { }
//			class DerivedClass : BaseClass { }
//			interface IBase { }
//			interface IDerived : IBase { }

//			[Fact]
//			public void TInterface_NotInterface_Throws()
//			{
//				Assert.Throws<TypeArgumentException>(
//					() => new BuilderSetup<BaseClass, DerivedClass>());
//			}

//			[Fact]
//			public void TImplementation_NotClass_Throws()
//			{
//				Assert.Throws<TypeArgumentException>(
//					() => new BuilderSetup<IBase, IDerived>());
//			}
//		}

//		public class UseImplementationMethod
//		{
//			[Fact]
//			public void Constructor_UsesPassedIn_TImplementation()
//			{
//				var runInfo = new TestRunInfo
//				{
//					Bool1 = true
//				};

//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = new BuilderSetup<ITestRunInfo, TestRunInfo>()
//					.UseImplementation(runInfo)
//					.Create();

//				builder.Store.AddCommand("command", context => ProcessStageResult.Success());

//				BuildResult<ITestRunInfo> result = builder.Build(new string[] { "command" });

//				Assert.True(result.RunInfo.Bool1);
//				Assert.Same(runInfo, result.RunInfo);
//			}

//			[Fact]
//			public void Constructor_TImplementationNotPassedIn_CreatesWithActivator()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				builder.Store.AddCommand("command", context => ProcessStageResult.Success());

//				BuildResult<ITestRunInfo> result = builder.Build(new string[] { "command" });

//				Assert.False(result.RunInfo.Bool1);
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				var runInfo = new TestRunInfo
//				{
//					Bool1 = true
//				};

//				var builder = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				BuilderSetup<ITestRunInfo, TestRunInfo>  returned = builder.UseImplementation(runInfo);

//				Assert.Same(builder, returned);
//			}
//		}

//		public class UseLoggerMethod
//		{
//			[Fact]
//			public void AlreadySet_Throws()
//			{
//				var builder = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				var logger = new TestLogger();

//				builder.UseLogger(logger);

//				Assert.Throws<InvalidOperationException>(() => builder.UseLogger(logger));
//			}

//			[Fact]
//			public void NullLoggerArgument_Throws()
//			{
//				var builder = new BuilderSetup<ITestRunInfo, TestRunInfo>();
//				Assert.Throws<ArgumentNullException>(() => builder.UseLogger(null));
//			}

//			[Fact]
//			public void ValidLoggerArgument_SuccessfullySets()
//			{
//				Assert.False(Logger.IsConfigured());

//				var builder = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				var logger = new TestLogger();

//				builder
//					.UseLogger(logger)
//					.Create();

//				Assert.True(Logger.IsConfigured());
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				var builder = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				var logger = new TestLogger();

//				BuilderSetup<ITestRunInfo, TestRunInfo>  returned = builder.UseLogger(logger);

//				Assert.Same(builder, returned);
//			}
//		}

//		public class UseOptionRegexMethod
//		{
//			[Fact]
//			public void AlreadySet_Throws()
//			{
//				var setup = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				setup.UseOptionRegex("regex");

//				Assert.Throws<InvalidOperationException>(() => setup.UseOptionRegex("regex"));
//			}

//			[Fact]
//			public void NullRegexArgument_Throws()
//			{
//				var setup = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				Assert.Throws<ArgumentNullException>(() => setup.UseOptionRegex(null));
//				Assert.Throws<ArgumentNullException>(() => setup.UseOptionRegex(""));
//			}

//			[Fact]
//			public void ValidInvoke_UsesConfiguredRegex()
//			{
//				var setupDefaultRegex = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				var defaultBuilder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				throw new NotImplementedException("TODO");
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				var setup = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				BuilderSetup<ITestRunInfo, TestRunInfo> returned = setup.UseOptionRegex("regex");

//				Assert.Same(setup, returned);
//			}
//		}

//		public class UseArgumentRegexMethod
//		{
//			[Fact]
//			public void AlreadySet_Throws()
//			{
//				var setup = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				setup.UseArgumentRegex("regex");

//				Assert.Throws<InvalidOperationException>(() => setup.UseArgumentRegex("regex"));
//			}

//			[Fact]
//			public void NullRegexArgument_Throws()
//			{
//				var setup = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				Assert.Throws<ArgumentNullException>(() => setup.UseArgumentRegex(null));
//				Assert.Throws<ArgumentNullException>(() => setup.UseArgumentRegex(""));
//			}

//			[Fact]
//			public void ValidInvoke_UsesConfiguredRegex()
//			{
//				var setupDefaultRegex = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				var defaultBuilder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				throw new NotImplementedException("TODO");
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				var setup = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				BuilderSetup<ITestRunInfo, TestRunInfo> returned = setup.UseArgumentRegex("regex");

//				Assert.Same(setup, returned);
//			}
//		}

//		public class HandleAllExceptionsMethod
//		{
//			[Fact]
//			public void Invoking_SuccessfullySets()
//			{
//				var setup = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builderNotHandles = setup.Create();
//				builderNotHandles.Store.AddCommand("command", context => throw new TestException());

//				var args = new string[] { "command" };

//				Assert.Throws<TestException>(() => builderNotHandles.Build(args));

//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builderHandles = setup
//					.HandleAllExceptions()
//					.Create();

//				builderHandles.Store.AddCommand("command", context => throw new TestException());

//				BuildResult<ITestRunInfo> buildResult = builderHandles.Build(args);
//				Assert.NotNull(buildResult.Exception);
//				Assert.Equal(BuildResultType.ExternalException, buildResult.Type);
//			}

//			[Fact]
//			public void Invoking_ReturnsItself()
//			{
//				var setup = new BuilderSetup<ITestRunInfo, TestRunInfo>();

//				BuilderSetup<ITestRunInfo, TestRunInfo> returned = setup.HandleAllExceptions();

//				Assert.Same(setup, returned);
//			}
//		}

//		public class DefaultStaticFactoryMethod
//		{
//			[Fact]
//			public void Invoking_ReturnsBuilder_WithExpectedSetup()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> defaultBuilder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				// Pre build assertions:
//				Assert.False(Logger.IsConfigured());
//				//Assert.True(UsesDefaultOptionRegex(defaultBuilder));
//				//Assert.True(UsesDefaultArgumentRegex(defaultBuilder));
//				Assert.False(HandlesAllExceptions(defaultBuilder));





//			}

//			private bool UsesPreConfiguredRunInfo(TestRunInfo info, IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder)
//			{
//				builder.Store.AddArgument("useinfo", context => ProcessStageResult.Success());
//				BuildResult<ITestRunInfo> buildResult = builder.Build(new string[] { "useinfo" });
//				return buildResult.RunInfo == info;
//			}

//			//private bool UsesDefaultOptionRegex(IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder)
//			//{
//			//	builder.Pipeline.
//			//}

//			//private bool UsesDefaultArgumentRegex(IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder)
//			//{

//			//}

//			private bool HandlesAllExceptions(IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder)
//			{
//				try
//				{
//					builder.Store.AddCommand("throw", context => throw new TestException());
//					builder.Build(new string[] { "throw" });
//					return true;
//				}
//				catch (Exception)
//				{
//					return false;
//				}
//			}
//		}
//	}
//}
