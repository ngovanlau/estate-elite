using EventBus.Infrastructures.Interfaces;
using EventBus.RabbitMQ.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Serilog;

namespace EventBus.RabbitMQ.Extensions;

public static class EventBusServicesExtension
{
    public static IServiceCollection AddEventBusServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register event bus subscription manager
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        var retryCount = 5;
        if (!int.TryParse(configuration["RabbitMQ:RetryCount"], out retryCount))
        {
            Log.Warning("Invalid retry count configured. Using default value {RetryCount}", retryCount);
        }

        // Configure RabbitMQ connection
        services.AddSingleton<IRabbitMQConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger>();

            var hostName = configuration["RabbitMQ:HostName"];
            if (string.IsNullOrWhiteSpace(hostName))
            {
                logger.Error("RabbitMQ hostname is not provided.");
                throw new ArgumentException("RabbitMQ hostname cannot be null or empty.", nameof(hostName));
            }

            var username = configuration["RabbitMQ:UserName"];
            if (string.IsNullOrWhiteSpace(username))
            {
                logger.Error("RabbitMQ username is not provided.");
                throw new ArgumentException("RabbitMQ username cannot be null or empty.", nameof(username));
            }

            var password = configuration["RabbitMQ:Password"];
            if (string.IsNullOrWhiteSpace(password))
            {
                logger.Error("RabbitMQ password is not provided.");
                throw new ArgumentException("RabbitMQ password cannot be null or empty.", nameof(password));
            }

            var virtualHost = configuration["RabbitMQ:VirtualHost"];
            if (string.IsNullOrWhiteSpace(virtualHost))
            {
                logger.Error("RabbitMQ virtual host is not provided.");
                throw new ArgumentException("RabbitMQ virtual host cannot be null or empty.", nameof(virtualHost));
            }

            var port = 5672; // Default port for RabbitMQ
            if (!int.TryParse(configuration["RabbitMQ:Port"], out port))
            {
                logger.Warning("Invalid RabbitMQ port configured. Using default port {Port}.", port);
            }

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = username,
                Password = password,
                VirtualHost = virtualHost,
                Port = port,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                TopologyRecoveryEnabled = true,
                ClientProvidedName = $"{configuration["ServiceName"]}_connection"
            };

            logger.Information("Configuring RabbitMQ persistent connection with {RetryCount} retries", retryCount);

            return new RabbitMQConnection(factory, logger, retryCount);
        });

        services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
        {
            var connection = sp.GetRequiredService<IRabbitMQConnection>();
            var logger = sp.GetRequiredService<ILogger>();
            var manager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
            var serviceName = configuration["ServiceName"] ?? "UnknownService";
            var queueName = $"{serviceName}_event_bus";

            return RabbitMQEventBus.CreateAsync(connection, logger, sp, manager, queueName, retryCount).GetAwaiter().GetResult();
        });

        return services;
    }
}
