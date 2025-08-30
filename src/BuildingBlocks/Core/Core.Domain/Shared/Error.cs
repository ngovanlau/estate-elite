/*
 * Why this Error record is designed this way:
 * -------------------------------------------
 * 1. Record Type:
 *    - `record` is used instead of `class` because Error is a value object
 *      (compared by values, immutable by default).
 *    - This ensures that two errors with the same Code/Message/Type are equal.
 *
 * 2. Sealed:
 *    - Prevents inheritance, keeping all error definitions consistent and centralized.
 *
 * 3. Predefined Instances (e.g., None, NullValue):
 *    - Provide common reusable errors without needing to recreate them.
 *    - `Error.None` avoids using null to represent "no error".
 *
 * 4. Static Factory Methods (e.g., NotFound, Validation, Conflict...):
 *    - Self-explanatory names make code more readable than `new Error(..., ErrorType.X)`.
 *    - Enforces correct ErrorType automatically (avoids mistakes).
 *    - Centralizes creation logic: if error initialization changes later (e.g., add logging),
 *      only the factory method needs updating.
 *
 * 5. Implicit Conversion to string + ToString override:
 *    - Allows using Error directly as string (returns Code).
 *    - Useful in logging, dictionaries, or simple checks.
 *
 * 6. ErrorType Enum:
 *    - Strongly typed categories of errors (None, Failure, Validation, NotFound, etc.).
 *    - Easy mapping to HTTP status codes or other layers.
 *
 * Overall:
 * This design follows Domain-Driven Design (DDD) best practices:
 * - Errors are value objects (records).
 * - Common errors are predefined.
 * - Static factories ensure consistency and readability.
 * - Provides type safety, avoids nulls, and makes error handling uniform across the system.
 */

namespace Core.Domain.Shared;

/// <summary>
/// Represents an error with code, message, and type.
/// </summary>
/// <param name="Code">Error code</param>
/// <param name="Message">Error message</param>
/// <param name="Type">Error type</param>
public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    /// <summary>
    /// Static Field
    /// Empty error representing no error
    /// </summary>
    public static readonly Error None
        = new(string.Empty, string.Empty, ErrorType.None);

    /// <summary>
    /// Generic null value error
    /// </summary>
    public static readonly Error NullValue
        = new("Error.NullValue", "The specified result value is null.");

    /// <summary>
    /// Static Method Factory
    /// Creates a not found error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Not found error</returns>
    public static Error NotFound(string code, string message)
        => new(code, message, ErrorType.NotFound);

    /// <summary>
    /// Creates a validation error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Validation error</returns>
    public static Error Validation(string code, string message)
        => new(code, message, ErrorType.Validation);

    /// <summary>
    /// Creates a conflict error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Conflict error</returns>
    public static Error Conflict(string code, string message)
        => new(code, message, ErrorType.Conflict);

    /// <summary>
    /// Creates an unauthorized error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Unauthorized error</returns>
    public static Error Unauthorized(string code, string message)
        => new(code, message, ErrorType.Unauthorized);

    /// <summary>
    /// Creates a forbidden error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Forbidden error</returns>
    public static Error Forbidden(string code, string message)
        => new(code, message, ErrorType.Forbidden);

    /// <summary>
    /// Implicit conversion to string (returns error code)
    /// </summary>
    /// <param name="error">Error to convert</param>
    /// <returns>Error code as string</returns>
    public static implicit operator string(Error error) => error.Code;

    /// <summary>
    /// Returns error code as string representation
    /// </summary>
    /// <returns>Error code</returns>
    public override string ToString() => Code;
}

/// <summary>
/// Types of errors that can occur
/// </summary>
public enum ErrorType
{
    None = 0,
    Failure = 1,
    Validation = 2,
    NotFound = 3,
    Conflict = 4,
    Unauthorized = 5,
    Forbidden = 6
}
