/*
 * Why this ApplicationException is designed this way:
 * ---------------------------------------------------
 * 1. Purpose:
 *    - Serves as a base exception for the application layer.
 *    - Provides structured error information for logging, APIs, or handling.
 *
 * 2. ErrorCode:
 *    - A string code to identify the type of error.
 *    - Useful for API consumers or consistent logging.
 *
 * 3. Details:
 *    - Dictionary to store additional contextual data about the error.
 *    - Can include invalid parameters, entity IDs, or state at failure time.
 *
 * 4. Chaining with inner exception:
 *    - Supports inner exceptions for full exception stack preservation.
 *
 * 5. Fluent WithDetail method:
 *    - Allows adding multiple details in a chainable, readable way:
 *        throw new SomeAppException("SomeCode", "Message")
 *                  .WithDetail("Id", entity.Id)
 *                  .WithDetail("Action", "Create");
 *
 * 6. Best Practices:
 *    - Always inherit from ApplicationException for domain/application-specific errors.
 *    - Avoid exposing sensitive information in Details for public APIs.
 *    - Use ErrorCode consistently across your application for mapping to HTTP responses.
 *
 * Overall:
 *    - This design ensures structured, consistent, and informative exception handling in the application layer.
 */

namespace Core.Application.Exceptions;

/// <summary>
/// Base exception for application layer
/// </summary>
public abstract class ApplicationException : Exception
{
    /// <summary>
    /// Error code for the exception
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Additional error details
    /// </summary>
    public Dictionary<string, object> Details { get; }

    /// <summary>
    /// Initializes a new instance of ApplicationException
    /// </summary>
    /// <param name="errorCode">Error code</param>
    /// <param name="message">Error message</param>
    protected ApplicationException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
        Details = [];
    }

    /// <summary>
    /// Initializes a new instance of ApplicationException with inner exception
    /// </summary>
    /// <param name="errorCode">Error code</param>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    protected ApplicationException(string errorCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Details = [];
    }

    /// <summary>
    /// Adds detail to the exception
    /// </summary>
    /// <param name="key">Detail key</param>
    /// <param name="value">Detail value</param>
    /// <returns>Current exception instance</returns>
    public ApplicationException WithDetail(string key, object value)
    {
        Details[key] = value;
        return this;
    }
}
