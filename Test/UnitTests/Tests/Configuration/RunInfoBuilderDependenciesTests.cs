using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.UnitTests.Tests.Configuration
{
    public class RunInfoBuilderDependenciesTests
    {
		[Fact]
		public void ConstructorParameters_MatchThatOf_RunInfoBuilder()
		{
			ConstructorInfo[] dependencyConstructors = typeof(RunInfoBuilderDependencies<TestRunInfo>).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
			Assert.Single(dependencyConstructors);
			ParameterInfo[] dependencyParameters = dependencyConstructors[0].GetParameters();

			ConstructorInfo[] builderConstructors = typeof(RunInfoBuilder<TestRunInfo>).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
			Assert.Single(builderConstructors);
			ParameterInfo[] builderParameters = builderConstructors[0].GetParameters();

			Assert.Equal(builderParameters.Length, dependencyParameters.Length);

			var requiredBuilderParameters = new HashSet<Type>(builderParameters.Select(p => p.ParameterType));

			dependencyParameters.ForEach(p => Assert.Contains(p.ParameterType, requiredBuilderParameters));
		}
		
    }
}
