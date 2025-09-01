/*
 * Why this Error record is designed this way:
 * -------------------------------------------
 * 1. Record Type:
 *    - `record` is used instead of `class` because Error is a value object:
 *        - Immutable by default.
 *        - Compared by values, not references.
 *    - Two errors with the same Code/Message/Type are automatically equal.
 *
 * 2. Sealed:
 *    - Prevents inheritance, ensuring all error definitions remain consistent and centralized.
 *
 * 3. Predefined Instances:
 *    - `None` represents the absence of an error.
 *    - `NullValue` is a common generic error to avoid using nulls.
 *
 * 4. Static Factory Methods:
 *    - Methods like `NotFound`, `Validation`, `Conflict`, `Forbidden`, `Unauthorized`:
 *        - Provide readable, self-explanatory error creation.
 *        - Automatically assign the correct ErrorType.
 *        - Centralize creation logic for consistency and future maintainability.
 *
 * 5. Details Dictionary:
 *    - Optional key-value store for additional context about the error.
 *    - Useful for logging, diagnostics, or API responses.
 *
 * 6. Implicit Conversion & ToString:
 *    - Allows using `Error` directly as a string (returns Code).
 *    - Simplifies logging, dictionary keys, and other operations.
 *
 * 7. ErrorType Enum:
 *    - Strongly typed categories of errors (Validation, NotFound, Conflict, etc.).
 *    - Facilitates mapping to HTTP status codes or other layers.
 *
 * Overall Design:
 *    - Follows DDD principles: errors are value objects, predefined where common, 
 *      and created via factories for consistency.
 *    - Ensures type safety, avoids nulls, and makes error handling uniform across the system.
 */

namespace Core.Domain.Shared;

/// <summary>
/// Represents an error with code, message, and type.
/// </summary>
/// <param name="Code">Error code</param>
/// <param name="Message">Error message</param>
/// <param name="Type">Error type</param>
public sealed record Error(
    string Code,
    string Message,
    ErrorType Type = ErrorType.Failure,
    Dictionary<string, object>? Details = null)
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
    public static Error NotFound(string code, string message, Dictionary<string, object>? details = null)
        => new(code, message, ErrorType.NotFound, details);

    /// <summary>
    /// Creates a validation error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Validation error</returns>
    public static Error Validation(string code, string message, Dictionary<string, object> details)
        => new(code, message, ErrorType.Validation, details);

    /// <summary>
    /// Creates a conflict error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Conflict error</returns>
    public static Error Conflict(string code, string message, Dictionary<string, object>? details = null)
        => new(code, message, ErrorType.Conflict, details);

    /// <summary>
    /// Creates an unauthorized error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Unauthorized error</returns>
    public static Error Unauthorized(string code, string message, Dictionary<string, object>? details = null)
        => new(code, message, ErrorType.Unauthorized, details);

    /// <summary>
    /// Creates a forbidden error
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Forbidden error</returns>
    public static Error Forbidden(string code, string message, Dictionary<string, object>? details = null)
        => new(code, message, ErrorType.Forbidden, details);

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
