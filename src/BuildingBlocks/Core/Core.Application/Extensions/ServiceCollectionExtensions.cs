/*
 * Why this ServiceCollectionExtensions class is designed this way:
 * ---------------------------------------------------------------
 * 1. Centralized Registration:
 *    - Provides a single place to register all Core Application services.
 *    - Reduces boilerplate in Startup/Program files.
 *
 * 2. MediatR Integration:
 *    - Registers all request handlers from the specified assembly automatically.
 *    - Adds pipeline behaviors in a consistent order: Exception Handling → Logging → Validation → Performance → Transaction.
 *    - Ensures cross-cutting concerns are handled consistently for all requests.
 *
 * 3. FluentValidation Integration:
 *    - Automatically scans and registers all validators in the same assembly.
 *    - Supports dependency injection and transient lifetime for validators.
 *
 * 4. Overload with Custom Behavior Configuration:
 *    - Provides flexibility for projects that need custom pipeline behaviors.
 *    - Consumers can inject or override behaviors without modifying the Core library.
 *
 * 5. Best Practices:
 *    - Keeps DI registration DRY and maintainable.
 *    - Enforces consistent MediatR pipeline across the application.
 *    - Decouples assembly scanning and registration from application entry point.
 *
 * Overall:
 * This design follows Clean Architecture principles:
 * - Promotes modularity and separation of concerns.
 * - Centralizes cross-cutting behavior registration.
 * - Makes the Core Application layer easy to plug into any host project.
 */

using System.Reflection;
using Core.Application.Behaviors;
using Core.Domain.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Extensions;

/// <summary>
/// Service collection extensions for Core Application
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Core Application services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="assembly">Assembly containing handlers</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddCoreApplication(
        this IServiceCollection services,
        Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            // Add MediatR
            config.RegisterServicesFromAssembly(assembly);

            // Add behaviors in order
            config.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            config.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        // Add FluentValidation
        services.AddValidatorsFromAssembly(assembly, ServiceLifetime.Transient);

        return services;
    }

    /// <summary>
    /// Adds Core Application services with custom behaviors
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="assembly">Assembly containing handlers</param>
    /// <param name="configureBehaviors">Action to configure behaviors</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddCoreApplication(
        this IServiceCollection services,
        Assembly assembly,
        Action<MediatRServiceConfiguration> configureBehaviors)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            configureBehaviors(config);
        });

        services.AddValidatorsFromAssembly(assembly, ServiceLifetime.Transient);

        return services;
    }
}