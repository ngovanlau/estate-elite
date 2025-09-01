/*
 * Why this ForbiddenException is designed this way:
 * -------------------------------------------------
 * 1. Purpose:
 *    - Represents authorization failures, e.g., when a user tries to access a resource or perform an action they are not allowed to.
 *    - Standardizes error handling for security-related checks in the application layer.
 *
 * 2. Inherits ApplicationException:
 *    - Consistent error code structure across the system.
 *    - Supports adding additional metadata via the `Details` dictionary.
 *
 * 3. Constructors:
 *    - Default message: provides a generic forbidden response.
 *    - Custom message: allows more descriptive error messages in specific contexts.
 *
 * 4. Static Factory Method (InsufficientPermissions):
 *    - Creates a standardized exception for permission issues.
 *    - Automatically attaches structured metadata (Resource and Action) for logging, diagnostics, or API response.
 *    - Improves readability and maintainability in services/handlers.
 *
 * 5. Details Property:
 *    - Stores structured info about the forbidden operation.
 *    - Useful for auditing, logging, or returning structured error responses in APIs.
 *
 * 6. Best Practices:
 *    - Throw ForbiddenException when user authentication succeeds but authorization fails.
 *    - Prefer static factory methods for common authorization failure scenarios to maintain consistency.
 *
 * Overall:
 *    - Provides a clear, type-safe, and consistent way to report authorization failures.
 *    - Enhances maintainability, logging clarity, and API error consistency.
 */

namespace Core.Application.Exceptions;

/// <summary>
/// Exception for authorization failures
/// </summary>
public sealed class ForbiddenException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of ForbiddenException
    /// </summary>
    /// <param name="message">Error message</param>
    public ForbiddenException(string message)
        : base("Forbidden.Access", message)
    {
    }

    /// <summary>
    /// Initializes a new instance of ForbiddenException with default message
    /// </summary>
    public ForbiddenException()
        : base("Forbidden.Access", "Access to this resource is forbidden.")
    {
    }

    /// <summary>
    /// Creates ForbiddenException for insufficient permissions
    /// </summary>
    /// <param name="resource">Resource name</param>
    /// <param name="action">Action attempted</param>
    /// <returns>ForbiddenException instance</returns>
    public static ForbiddenException InsufficientPermissions(string resource, string action)
    {
        var exception = new ForbiddenException(
            $"Insufficient permissions to {action} {resource}.");
        
        return (ForbiddenException)exception
            .WithDetail("Resource", resource)
            .WithDetail("Action", action);
    }
}