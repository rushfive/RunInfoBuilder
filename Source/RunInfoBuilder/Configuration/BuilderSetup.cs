using Microsoft.Extensions.DependencyInjection;
using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Pipeline;
using R5.RunInfoBuilder.Validators;
using R5.RunInfoBuilder.Version;
using System;

namespace R5.RunInfoBuilder.Configuration
{
	public class BuilderSetup<TRunInfo>
		where TRunInfo : class
	{
		public CommandConfigBuilder Commands { get; }
		public OptionConfigBuilder Options { get; }
		public ArgumentConfigBuilder Arguments { get; }
		public ParserConfigBuilder Parser { get; }
		public ProcessConfigBuilder<TRunInfo> Process { get; }

		private TRunInfo _implementation { get; set; }
		private HelpConfigBuilder<TRunInfo> _helpBuilder { get; set; }
		private VersionConfigBuilder _versionBuilder { get; set; }

		public BuilderSetup()
		{
			Commands = new CommandConfigBuilder();
			Options = new OptionConfigBuilder();
			Arguments = new ArgumentConfigBuilder();
			Parser = new ParserConfigBuilder();
			Process = new ProcessConfigBuilder<TRunInfo>();

			_implementation = null;

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

			(ProcessConfig processConfig, ProcessHooksConfig<TRunInfo> processHooks) = Process.Build();

			services
				.AddScoped<CommandConfig>(sp => Commands.Build())
				.AddScoped<OptionConfig>(sp => Options.Build())
				.AddScoped<ArgumentConfig>(sp => Arguments.Build())
				.AddScoped<ProcessConfig>(sp => processConfig)
				.AddScoped<RunInfo<TRunInfo>>(sp =>
				{
					TRunInfo implementation = _implementation ?? (TRunInfo)Activator.CreateInstance(typeof(TRunInfo));
					return new RunInfo<TRunInfo>(implementation);
				})
				.AddScoped<IParser>(sp =>
				{
					ParserConfig config = Parser.Build();
					return new Parser().Configure(config);
				})
				.AddScoped<IRestrictedKeyValidator, RestrictedKeyValidator>()
				.AddScoped<IReflectionHelper<TRunInfo>, ReflectionHelper<TRunInfo>>()
				.AddScoped<IArgumentMetadataMaps<TRunInfo>, ArgumentMetadataMaps<TRunInfo>>()
				.AddScoped<IArgumentStore<TRunInfo>, ArgumentStore<TRunInfo>>()
				.AddScoped<IArgumentTypeResolver, ArgumentTypeResolver<TRunInfo>>()
				.AddScoped<IValidationRuleSetFactory, ValidationRuleSetFactory<TRunInfo>>()
				.AddScoped<IValidationHelper, ValidationHelper<TRunInfo>>()
				.AddScoped<IArgumentStoreValidator<TRunInfo>, ArgumentStoreValidator<TRunInfo>>()
				.AddScoped<IBuildValidator, BuildValidator<TRunInfo>>()
				.AddScoped<IArgumentTokenizer, ArgumentTokenizer>()
				.AddHelpManager(_helpBuilder)
				.AddVersionManager(_versionBuilder)
				.AddPipelineProcessor(processHooks)
				.AddScoped<RunInfoBuilder<TRunInfo>>(sp =>
				{
					RunInfoBuilderDependencies<TRunInfo> dependencies = sp.GetBuilderDependencies<TRunInfo>();

					return new RunInfoBuilder<TRunInfo>(
						dependencies.Parser,
						dependencies.Pipeline,
						dependencies.Store,
						dependencies.BuildValidator,
						dependencies.HelpManager,
						dependencies.RunInfo,
						dependencies.VersionManager);
				});

			return services;
		}
	}

	// TODO: unit tests using reflecting to ensure that all the RunInfoBuilder
	// constructor parameters are accoutned for via properties in this class
	internal class RunInfoBuilderDependencies<TRunInfo>
		where TRunInfo : class
	{
		internal IParser Parser { get; }
		internal IPipelineProcessor<TRunInfo> Pipeline { get; }
		internal IArgumentStore<TRunInfo> Store { get; }
		internal IBuildValidator BuildValidator { get; }
		internal IHelpManager<TRunInfo> HelpManager { get; }
		internal RunInfo<TRunInfo> RunInfo { get; }
		internal IVersionManager VersionManager { get; }

		internal RunInfoBuilderDependencies(
			IParser parser,
			IPipelineProcessor<TRunInfo> pipeline,
			IArgumentStore<TRunInfo> store,
			IBuildValidator buildValidator,
			IHelpManager<TRunInfo> helpManager,
			RunInfo<TRunInfo> runInfo,
			IVersionManager versionManager)
		{
			Parser = parser;
			Pipeline = pipeline;
			Store = store;
			BuildValidator = buildValidator;
			HelpManager = helpManager;
			RunInfo = runInfo;
			VersionManager = versionManager;
		}
	}

	internal static class BuilderSetupExtensions
	{
		public static IServiceCollection AddHelpManager<TRunInfo>(this IServiceCollection services,
			HelpConfigBuilder<TRunInfo> helpBuilder)
			where TRunInfo : class
		{
			if (helpBuilder == null || !helpBuilder.IsValid())
			{
				return services.AddScoped<IHelpManager<TRunInfo>>(sp => null);
			}

			return services.AddScoped<IHelpManager<TRunInfo>, HelpManager<TRunInfo>>(sp =>
			{
				HelpConfig<TRunInfo> config = helpBuilder.Build();

				var argumentMaps = sp.GetRequiredService<IArgumentMetadataMaps<TRunInfo>>();
				var keyValidator = sp.GetRequiredService<IRestrictedKeyValidator>();

				return new HelpManager<TRunInfo>(argumentMaps, keyValidator).Configure(config);
			});
		}

		public static IServiceCollection AddVersionManager(this IServiceCollection services,
			VersionConfigBuilder versionBuilder)
		{
			if (versionBuilder == null || !versionBuilder.IsValid())
			{
				return services.AddScoped<IVersionManager>(sp => null);
			}

			return services.AddScoped<IVersionManager, VersionManager>(sp =>
			{
				VersionConfig config = versionBuilder.Build();

				var keyValidator = sp.GetRequiredService<IRestrictedKeyValidator>();

				return new VersionManager(keyValidator).Configure(config);
			});
		}

		public static IServiceCollection AddPipelineProcessor<TRunInfo>(this IServiceCollection services,
			ProcessHooksConfig<TRunInfo> config)
			where TRunInfo : class
		{
			return services.AddScoped<IPipelineProcessor<TRunInfo>, PipelineProcessor<TRunInfo>>(sp =>
			{
				var argumentMaps = sp.GetRequiredService<IArgumentMetadataMaps<TRunInfo>>();
				var runInfo = sp.GetRequiredService<RunInfo<TRunInfo>>();
				var parser = sp.GetRequiredService<IParser>();
				var tokenizer = sp.GetRequiredService<IArgumentTokenizer>();
				var processConfig = sp.GetRequiredService<ProcessConfig>();

				var pipeline = new PipelineProcessor<TRunInfo>(
					argumentMaps,
					runInfo,
					parser,
					tokenizer,
					processConfig);

				return pipeline.Configure(config);
			});
		}

		public static RunInfoBuilderDependencies<TRunInfo> GetBuilderDependencies<TRunInfo>(this IServiceProvider provider)
			where TRunInfo : class
		{
			var parser = provider.GetRequiredService<IParser>();
			var pipeline = provider.GetRequiredService<IPipelineProcessor<TRunInfo>>();
			var store = provider.GetRequiredService<IArgumentStore<TRunInfo>>();
			var buildValidator = provider.GetRequiredService<IBuildValidator>();
			var helpManager = provider.GetService<IHelpManager<TRunInfo>>();
			var runInfoValue = provider.GetRequiredService<RunInfo<TRunInfo>>();
			var versionManager = provider.GetService<IVersionManager>();

			return new RunInfoBuilderDependencies<TRunInfo>(
				parser,
				pipeline,
				store,
				buildValidator,
				helpManager,
				runInfoValue,
				versionManager);
		}
	}
}
