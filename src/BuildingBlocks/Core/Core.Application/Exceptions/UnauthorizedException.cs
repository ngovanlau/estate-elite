/*
 * Why this UnauthorizedException is designed this way:
 * -----------------------------------------------------
 * 1. Purpose:
 *    - Represents authentication failures when a user is not logged in or lacks credentials.
 *    - Separates authorization (Forbidden) from authentication (Unauthorized) for clarity and proper HTTP status mapping.
 *
 * 2. Inheritance:
 *    - Inherits from ApplicationException to integrate with the application-wide error handling framework.
 *    - Leverages ErrorCode and Details properties for structured error reporting.
 *
 * 3. Constructors:
 *    - Default constructor provides a standard message for generic unauthorized access.
 *    - Overloaded constructor allows custom messages for more specific contexts.
 *
 * 4. Static Factory Method:
 *    - `ForResource` creates a preconfigured exception for a given resource/action.
 *    - Automatically adds structured details (Resource, Action) to the exception for logging or diagnostics.
 *
 * 5. Benefits:
 *    - Promotes consistent error handling across the application.
 *    - Supports clean separation of authentication vs. authorization failures.
 *    - Integrates seamlessly with Result/Error patterns and MediatR exception pipeline.
 *
 * 6. Best Practices:
 *    - Throw UnauthorizedException when a user is unauthenticated.
 *    - Use ForbiddenException when a user is authenticated but lacks permissions.
 *    - Include context details using `WithDetail` or static factory methods for better observability.
 */

namespace Core.Application.Exceptions;

/// <summary>
/// Exception for authorization failures (user not authenticated)
/// </summary>
public sealed class UnauthorizedException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of UnauthorizedException
    /// </summary>
    /// <param name="message">Error message</param>
    public UnauthorizedException(string message)
        : base("Unauthorized.Access", message)
    {
    }

    /// <summary>
    /// Initializes a new instance of UnauthorizedException with default message
    /// </summary>
    public UnauthorizedException()
        : base("Unauthorized.Access", "Authentication is required to access this resource.")
    {
    }

    /// <summary>
    /// Creates UnauthorizedException for a specific resource and action
    /// </summary>
    /// <param name="resource">Resource name</param>
    /// <param name="action">Action attempted</param>
    /// <returns>UnauthorizedException instance</returns>
    public static UnauthorizedException ForResource(string resource, string action)
    {
        var exception = new UnauthorizedException(
            $"User is not authenticated to perform '{action}' on resource '{resource}'.");

        return (UnauthorizedException)exception
            .WithDetail("Resource", resource)
            .WithDetail("Action", action);
    }
}
