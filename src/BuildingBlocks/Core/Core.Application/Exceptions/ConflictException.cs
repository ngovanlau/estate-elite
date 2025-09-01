/*
 * Why this ConflictException is designed this way:
 * ------------------------------------------------
 * 1. Purpose:
 *    - Represents business rule violations or conflicts (e.g., duplicates, invalid state changes).
 *    - Provides structured, type-safe information for logging and API responses.
 *
 * 2. Inherits ApplicationException:
 *    - Consistent error code and detail structure across the application.
 *    - Supports attaching additional metadata via the `Details` dictionary.
 *
 * 3. Constructors:
 *    - General message: for ad-hoc conflicts.
 *    - Error code + message: allows categorizing conflicts systematically.
 *
 * 4. Static Factory Method (Duplicate):
 *    - Simplifies creation of standardized duplicate-entity conflicts.
 *    - Automatically sets error code, message, and structured details.
 *    - Improves readability in services/handlers.
 *
 * 5. Details Property:
 *    - Stores structured metadata like entity name, property, and value.
 *    - Useful for logging, diagnostics, or API payloads.
 *
 * 6. Best Practices:
 *    - Throw ConflictException when domain rules prevent an operation.
 *    - Prefer static factories for common scenarios to maintain consistency.
 *
 * Overall:
 *    - Provides a clear, standardized way to report conflicts.
 *    - Enhances maintainability, logging clarity, and API error consistency.
 */

namespace Core.Application.Exceptions;

/// <summary>
/// Exception for business rule violations or conflicts
/// </summary>
public sealed class ConflictException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of ConflictException
    /// </summary>
    /// <param name="message">Error message</param>
    public ConflictException(string message)
        : base("Conflict.General", message)
    {
    }

    /// <summary>
    /// Initializes a new instance of ConflictException with error code
    /// </summary>
    /// <param name="errorCode">Error code</param>
    /// <param name="message">Error message</param>
    public ConflictException(string errorCode, string message)
        : base(errorCode, message)
    {
    }

    /// <summary>
    /// Creates ConflictException for duplicate entity
    /// </summary>
    /// <param name="entityName">Entity name</param>
    /// <param name="propertyName">Property name that caused conflict</param>
    /// <param name="value">Property value</param>
    /// <returns>ConflictException instance</returns>
    public static ConflictException Duplicate(string entityName, string propertyName, object value)
    {
        var exception = new ConflictException(
            "Conflict.Duplicate",
            $"{entityName} with {propertyName} '{value}' already exists.");

        return (ConflictException)exception
            .WithDetail("EntityName", entityName)
            .WithDetail("PropertyName", propertyName)
            .WithDetail("Value", value);
    }
}
