using Microsoft.Extensions.DependencyInjection;
using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Validators;
using R5.RunInfoBuilder.Version;
using System;
using R5.RunInfoBuilder.Process;

namespace R5.RunInfoBuilder.Configuration
{
	public class BuilderSetup<TRunInfo>
		where TRunInfo : class
	{
		public CommandConfigBuilder Commands { get; }
		internal OptionConfigBuilder Options { get; }
		internal ArgumentConfigBuilder Arguments { get; }
		public ParserConfigBuilder Parser { get; }
		public ProcessConfigBuilder<TRunInfo> Process { get; }
		public ProcessHooksBuilder<TRunInfo> Hooks { get; }

		private TRunInfo _implementation { get; set; }
		private bool _alwaysReturnBuildResult { get; set; }

		private HelpConfigBuilder<TRunInfo> _helpBuilder { get; set; }
		private VersionConfigBuilder _versionBuilder { get; set; }

		public BuilderSetup()
		{
			Commands = new CommandConfigBuilder();
			Options = new OptionConfigBuilder();
			Arguments = new ArgumentConfigBuilder();
			Parser = new ParserConfigBuilder();
			Process = new ProcessConfigBuilder<TRunInfo>();
			Hooks = new ProcessHooksBuilder<TRunInfo>();

			_implementation = null;
			_alwaysReturnBuildResult = false;

			// these two are configuration through method calls because they're optional
			_helpBuilder = null;
			_versionBuilder = null;
		}

		public BuilderSetup<TRunInfo> UseImplementation(TRunInfo implementation)
		{
			if (_implementation != null)
			{
				throw new InvalidOperationException("Run info implementation has already been set.");
			}

			_implementation = implementation ?? throw new ArgumentNullException(nameof(implementation), "Run info implementation must be provided.");
			return this;
		}

		public BuilderSetup<TRunInfo> AlwaysReturnBuildResult()
		{
			_alwaysReturnBuildResult = true;
			return this;
		}

		public BuilderSetup<TRunInfo> ConfigureHelp(Action<HelpConfigBuilder<TRunInfo>> config)
		{
			if (_helpBuilder != null)
			{
				throw new InvalidOperationException("Help configuration can only be setup once.");
			}
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config), "Help manager configuration must be provided.");
			}

			_helpBuilder = new HelpConfigBuilder<TRunInfo>();

			config(_helpBuilder);

			return this;
		}

		public BuilderSetup<TRunInfo> ConfigureVersion(Action<VersionConfigBuilder> config)
		{
			if (_versionBuilder != null)
			{
				throw new InvalidOperationException("Version configuration can only be setup once.");
			}
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config), "Version manager configuration must be provided.");
			}

			_versionBuilder = new VersionConfigBuilder();

			config(_versionBuilder);

			return this;
		}

		public RunInfoBuilder<TRunInfo> Create()
		{
			return ResolveServices()
				.BuildServiceProvider()
				.GetRequiredService<RunInfoBuilder<TRunInfo>>();
		}

		// maybe make this return serviceProvider?
		// maybe ONLY add IRunInfoBuilder if via ioc/DI
		// TODO: refactor this to make more readable (split service adds into better groups?)
		internal IServiceCollection ResolveServices()
		{
			IServiceCollection services = new ServiceCollection();

			services
				.AddConfigs(this)
				.AddParserServices()
				.AddHelpServices(_helpBuilder)
				.AddProcessServices<TRunInfo>()
				.AddStoreServices<TRunInfo>()
				.AddVersionServices(_versionBuilder)
				.AddValidators<TRunInfo>()
				.AddScoped<RunInfo<TRunInfo>>(sp =>
				{
					TRunInfo implementation = _implementation ?? (TRunInfo)Activator.CreateInstance(typeof(TRunInfo));
					return new RunInfo<TRunInfo>(implementation);
				})
				.AddScoped<RunInfoBuilder<TRunInfo>>(sp =>
				{
					RunInfoBuilderDependencies<TRunInfo> dependencies = sp.GetRunInfoBuilderDependencies<TRunInfo>();

					return new RunInfoBuilder<TRunInfo>(
						dependencies.ProcessInvoker,
						dependencies.Parser,
						dependencies.Store,
						dependencies.BuildValidator,
						dependencies.RunInfo,
						dependencies.Config,
						dependencies.HooksConfig);
				});

			return services;
		}

		internal BuilderConfig BuildConfig()
		{
			return new BuilderConfig(_alwaysReturnBuildResult);
		}
	}

	internal static class BuilderSetupExtensions
	{
		public static IServiceCollection AddConfigs<TRunInfo>(this IServiceCollection services, BuilderSetup<TRunInfo> setup)
			where TRunInfo : class
		{
			return services
				.AddScoped<CommandConfig>(sp => setup.Commands.Build())
				.AddScoped<OptionConfig>(sp => setup.Options.Build())
				.AddScoped<ArgumentConfig>(sp => setup.Arguments.Build())
				.AddScoped<ProcessConfig>(sp => setup.Process.Build())
				.AddScoped<HooksConfig<TRunInfo>>(sp => setup.Hooks.Build())
				.AddScoped<BuilderConfig>(sp => setup.BuildConfig())
				.AddScoped<ParserConfig>(sp => setup.Parser.Build());
		}

		public static IServiceCollection AddParserServices(this IServiceCollection services)
		{
			return services
				.AddScoped<IParser, Parser>();
		}

		public static IServiceCollection AddHelpServices<TRunInfo>(this IServiceCollection services,
			HelpConfigBuilder<TRunInfo> helpBuilder)
			where TRunInfo : class
		{
			if (helpBuilder == null)
			{
				return services.AddScoped<IHelpManager<TRunInfo>>(sp => null);
			}

			return services
				.AddScoped<HelpConfig<TRunInfo>>(sp => helpBuilder.Build())
				.AddScoped<IHelpManager<TRunInfo>, HelpManager<TRunInfo>>();
		}

		public static IServiceCollection AddProcessServices<TRunInfo>(this IServiceCollection services)
			where TRunInfo : class
		{
			return services
				.AddScoped<IProcessInvoker, ProcessInvoker<TRunInfo>>()
				.AddScoped<IStageChainFactory<TRunInfo>, StageChainFactory<TRunInfo>>()
				.AddScoped<IValidationContextFactory, ValidationContextFactory>();
		}

		public static IServiceCollection AddStoreServices<TRunInfo>(this IServiceCollection services)
			where TRunInfo : class
		{
			return services
				.AddScoped<IArgumentMetadata<TRunInfo>, ArgumentMetadata<TRunInfo>>()
				.AddScoped<IArgumentStore<TRunInfo>, ArgumentStore<TRunInfo>>()
				.AddScoped<IArgumentTypeResolver, ArgumentTypeResolver<TRunInfo>>()
				.AddScoped<IArgumentTokenizer, ArgumentTokenizer>()
				.AddScoped<IArgumentStoreValidator<TRunInfo>, ArgumentStoreValidator<TRunInfo>>()
				.AddScoped<IReflectionHelper<TRunInfo>, ReflectionHelper<TRunInfo>>();
		}

		public static IServiceCollection AddVersionServices(this IServiceCollection services,
			VersionConfigBuilder versionBuilder)
		{
			if (versionBuilder == null)
			{
				return services.AddScoped<IVersionManager>(sp => null);
			}

			return services
				.AddScoped<VersionConfig>(sp => versionBuilder.Build())
				.AddScoped<IVersionManager, VersionManager>();
		}

		public static IServiceCollection AddValidators<TRunInfo>(this IServiceCollection services)
			where TRunInfo : class
		{
			return services
				.AddScoped<IRestrictedKeyValidator, RestrictedKeyValidator>()
				//.AddScoped<IValidationRuleSetFactory, ValidationRuleSetFactory<TRunInfo>>()
				.AddScoped<IArgumentStoreValidator<TRunInfo>, ArgumentStoreValidator<TRunInfo>>()
				.AddScoped<IBuildValidator, BuildValidator<TRunInfo>>();
		}

		public static RunInfoBuilderDependencies<TRunInfo> GetRunInfoBuilderDependencies<TRunInfo>(this IServiceProvider provider)
			where TRunInfo : class
		{
			var processInvoker = provider.GetRequiredService<IProcessInvoker>();
			var parser = provider.GetRequiredService<IParser>();
			var store = provider.GetRequiredService<IArgumentStore<TRunInfo>>();
			var buildValidator = provider.GetRequiredService<IBuildValidator>();
			var runInfoValue = provider.GetRequiredService<RunInfo<TRunInfo>>();
			var config = provider.GetRequiredService<BuilderConfig>();
			var hooksConfig = provider.GetRequiredService<HooksConfig<TRunInfo>>();

			return new RunInfoBuilderDependencies<TRunInfo>(
				processInvoker,
				parser,
				store,
				buildValidator,
				runInfoValue,
				config,
				hooksConfig);
		}
	}
}
