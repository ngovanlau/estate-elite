/*
 * Why define ValidationResult and ValidationResult<T>:
 * ---------------------------------------------------
 * 1. Purpose:
 *    - Encapsulates multiple validation errors instead of a single error.
 *    - Used when input validation or business rules produce multiple failures.
 *    - Extends the Result pattern for validation scenarios.
 *
 * 2. Two Variants:
 *    - ValidationResult:
 *        • Used when operation returns no payload (just success/failure).
 *    - ValidationResult<T>:
 *        • Used when operation returns a payload (Result<T>).
 *        • Example: Query or Command returning a DTO but validation failed.
 *
 * 3. Implementation:
 *    - Both inherit from Result / Result<T>.
 *    - Always represent a failure (base(false, ValidationError)).
 *    - Store a collection of validation errors (Error[] Errors).
 *    - Factory method (WithErrors) ensures consistent creation.
 *
 * 4. IValidationResult Contract:
 *    - Provides standard ValidationError (Error.Validation).
 *    - Enforces Errors[] property.
 *
 * 5. Benefits:
 *    - Clean way to separate validation errors from business errors.
 *    - Supports consistent error handling across application.
 *    - Makes commands/queries validation-friendly:
 *        • Example: pipeline behavior in MediatR can return ValidationResult directly.
 *
 * Overall:
 * ValidationResult and ValidationResult<T> provide a structured,
 * reusable approach to handling multiple validation failures within
 * the Result pattern, improving consistency and clarity.
 */

namespace Core.Domain.Shared;

public sealed class ValidationResult : Result, IValidationResult
{
    /// <inheritdoc />
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors) : base(false, IValidationResult.ValidationError)
        => Errors = errors;

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

/// <summary>
/// Represents a validation result with a payload (used when TResponse = Result<T>).
/// </summary>
public sealed class ValidationResult<T> : Result<T>, IValidationResult
{
    /// <inheritdoc />
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors)
        : base(default!, false, IValidationResult.ValidationError)
        => Errors = errors;

    /// <summary>
    /// Factory method to create a ValidationResult with errors.
    /// </summary>
    public static ValidationResult<T> WithErrors(Error[] errors) => new(errors);
}

/// <summary>
/// Contract for validation result.
/// </summary>
public interface IValidationResult
{
    /// <summary>
    /// Standard validation error
    /// </summary>
    public static readonly Error ValidationError = Error.Validation(
        "ValidationError", "A validation error occurred.");

    /// <summary>
    /// Array of validation errors
    /// </summary>
    Error[] Errors { get; }
}