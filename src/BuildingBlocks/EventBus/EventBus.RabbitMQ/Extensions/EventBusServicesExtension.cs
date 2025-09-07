using EventBus.Abstraction.Interfaces;
using EventBus.RabbitMQ.Interfaces;
using EventBus.RabbitMQ.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBus.RabbitMQ.Extensions;

public static class EventBusServicesExtension
{
    public static IServiceCollection AddEventBusServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register event bus subscription manager
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        // Configure RabbitMQ connection
        services.AddSingleton<IRabbitMQConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQConnection>>();
            var rabbitMQSetting = configuration.GetRequiredSection("RabbitMQ").Get<RabbitMQSetting>();

            if (rabbitMQSetting == null)
            {
                logger.LogError("RabbitMQ settings are not configured.");
                throw new ArgumentNullException(nameof(rabbitMQSetting), "RabbitMQ settings cannot be null.");
            }

            var factory = new ConnectionFactory
            {
                HostName = rabbitMQSetting.HostName,
                UserName = rabbitMQSetting.UserName,
                Password = rabbitMQSetting.Password,
                VirtualHost = rabbitMQSetting.VirtualHost,
                Port = rabbitMQSetting.Port,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                TopologyRecoveryEnabled = true,
                ClientProvidedName = $"{configuration["ServiceName"]}_connection"
            };

            logger.LogInformation("Configuring RabbitMQ persistent connection with {RetryCount} retries", rabbitMQSetting.RetryCount);

            return new RabbitMQConnection(factory, logger, rabbitMQSetting.RetryCount);
        });

        services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
        {
            var serviceName = configuration.GetValue("ServiceName", "UnknownService");
            var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var connection = sp.GetRequiredService<IRabbitMQConnection>();
            var manager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
            var queueName = $"{serviceName}_event_bus";
            var retryCount = configuration.GetValue("RabbitMQ:RetryCount", 5);

            logger.LogInformation("Creating RabbitMQ EventBus with Queue: {QueueName}", queueName);

            return RabbitMQEventBus.CreateAsync(connection, logger, sp, manager, queueName, retryCount).GetAwaiter().GetResult();
        });

        return services;
    }
}
