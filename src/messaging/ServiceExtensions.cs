using System;
using CSGOStats.Extensions.Extensions;
using CSGOStats.Extensions.Validation;
using CSGOStats.Infrastructure.Messaging.Config;
using CSGOStats.Infrastructure.Messaging.Handling;
using CSGOStats.Infrastructure.Messaging.Handling.Pipeline;
using CSGOStats.Infrastructure.Messaging.Handling.Pipeline.StandardPipes;
using CSGOStats.Infrastructure.Messaging.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSGOStats.Infrastructure.Messaging
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfigurationRoot configuration) =>
            services
                .RegisterPipeline()
                .RegisterBus(configuration)
                .AddSingleton(_ => configuration.GetFromConfiguration(
                    sectionName: "PipelineRetry",
                    configurationSection => new RetrySetting(
                        retryCount: configurationSection["RetryCount"].Int())));

        public static IServiceCollection AddHandlers<TAssembly>(this IServiceCollection services) =>
            services.Scan(selector =>
                selector.FromAssemblyOf<TAssembly>()
                    .AddClasses(classes => classes.AssignableTo(typeof(BaseMessageHandler<>)))
                    .As<IHandler>()
                    .WithTransientLifetime());

        private static IServiceCollection RegisterPipeline(this IServiceCollection services) =>
            services
                .AddSingleton<IPipe, LoggingPipe>()
                .AddSingleton<IPipe, TimeMeasurePipe>();

        private static IServiceCollection RegisterBus(this IServiceCollection services, IConfigurationRoot configuration) =>
            services
                .AddScoped<IEventBus, RabbitMqEventBus>()
                .AddScoped<IMessageRegistrar, RabbitMqEventBus>()
                .AddScoped(provider => new RabbitMqEventBus(
                    configuration: provider.GetService<RabbitMqConnectionConfiguration>(),
                    serviceProvider: provider))
                .ConfigureRabbitMqConnectionSetting(configuration);

        private static IServiceCollection ConfigureRabbitMqConnectionSetting(
            this IServiceCollection serviceProvider,
            IConfigurationRoot configuration) =>
                serviceProvider.AddSingleton(_ =>
                    configuration.GetFromConfiguration(
                        sectionName: "RabbitMqConnection",
                        creatingFunctor: configurationSection =>
                            new RabbitMqConnectionConfiguration(
                                host: configurationSection["Host"],
                                port: configurationSection["Port"].Int(),
                                username: configurationSection["Username"],
                                password: configurationSection["Password"],
                                heartbeat: configurationSection["Heartbeat"].Int())));

        // todo: to core package
        private static TSetting GetFromConfiguration<TSetting>(
            this IConfigurationRoot configuration,
            string sectionName,
            Func<IConfiguration, TSetting> creatingFunctor)
            where TSetting : class
        {
            var section = configuration.NotNull(nameof(configuration)).GetSection(sectionName.NotNull(nameof(sectionName)));
            return creatingFunctor.NotNull(nameof(creatingFunctor)).Invoke(section);
        }
    }
}