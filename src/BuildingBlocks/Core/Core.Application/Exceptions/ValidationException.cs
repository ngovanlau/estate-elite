/*
 * Why this ValidationException is designed this way:
 * --------------------------------------------------
 * 1. Purpose:
 *    - Represents validation failures in the application layer.
 *    - Integrates both FluentValidation results and domain errors.
 *
 * 2. Inherits ApplicationException:
 *    - Provides consistent error code and optional details across the application.
 *
 * 3. Errors Property:
 *    - IReadOnlyDictionary<string, string[]> to map property names or error codes to messages.
 *    - Enables structured error responses for APIs or logging.
 *
 * 4. Multiple Constructors:
 *    - Default: empty validation result.
 *    - From FluentValidation failures: converts ValidationFailure list to structured dictionary.
 *    - From IReadOnlyDictionary: allows custom error dictionaries.
 *    - From domain Error[]: integrates domain-level validation with same structure.
 *
 * 5. Single Factory Method:
 *    - `Single` simplifies creation of a single-property error for quick validation.
 *    - Improves readability in command handlers or services.
 *
 * 6. Best Practices:
 *    - Always throw ValidationException for input validation failures.
 *    - Keep property keys consistent with API field names or domain properties.
 *    - Avoid exposing sensitive data in error messages returned to clients.
 *
 * Overall:
 *    - This design ensures structured, readable, and consistent validation error handling.
 *    - Works seamlessly with both FluentValidation and domain-level validations.
 */

using Core.Domain.Shared;
using FluentValidation.Results;

namespace Core.Application.Exceptions;

/// <summary>
/// Exception for validation failures
/// </summary>
public sealed class ValidationException : ApplicationException
{
    /// <summary>
    /// Validation errors
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Initializes a new instance of ValidationException
    /// </summary>
    public ValidationException()
        : base("Validation.General", "One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Initializes a new instance of ValidationException with validation failures
    /// </summary>
    /// <param name="failures">Validation failures</param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    /// <summary>
    /// Initializes a new instance of ValidationException with errors
    /// </summary>
    /// <param name="errors">Validation errors</param>
    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : this()
    {
        Errors = errors;
    }

    /// <summary>
    /// Initializes a new instance of ValidationException from Error array
    /// </summary>
    /// <param name="errors">Domain errors</param>
    public ValidationException(Error[] errors)
        : this()
    {
        Errors = errors
            .GroupBy(e => e.Code, e => e.Message)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    /// <summary>
    /// Creates ValidationException with single error
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <param name="errorMessage">Error message</param>
    /// <returns>ValidationException instance</returns>
    public static ValidationException Single(string propertyName, string errorMessage)
        => new ValidationException(new Dictionary<string, string[]>
        {
            [propertyName] = [errorMessage]
        });
}
