/*
 * Why define ValidationResult:
 * ----------------------------
 * 1. Purpose:
 *    - In DDD / Clean Architecture, validation often produces
 *      multiple errors at once (e.g., input validation).
 *    - Normal Result only carries ONE Error.
 *    - ValidationResult extends Result to support an array of Errors.
 *
 * 2. Implementation:
 *    - Inherits from Result:
 *        - Always Failure (base(false, ValidationError)).
 *    - Errors property holds multiple Error objects.
 *    - Constructor is private → ensures creation only via WithErrors().
 *
 * 3. IValidationResult interface:
 *    - Defines contract: ValidationError (static Error) + Errors[].
 *    - ValidationError = standardized error code ("ValidationError").
 *    - Prevents mixing business failure with validation failure.
 *
 * 4. Factory:
 *    - ValidationResult.WithErrors(errors) → clean API for building failure results.
 *    - Example:
 *        var result = ValidationResult.WithErrors(new[] {
 *            Error.Validation("FirstName", "First name is required."),
 *            Error.Validation("Age", "Age must be greater than 18.")
 *        });
 *
 * 5. Usage in Application Layer:
 *    - Instead of throwing ValidationException,
 *      return ValidationResult to the caller.
 *    - Allows API/UI layer to present detailed validation feedback.
 *
 * 6. Benefit:
 *    - Keeps validation handling consistent with Result pattern.
 *    - Prevents scattering of validation error handling.
 *    - Supports multiple errors → unlike simple Result<>, which carries only one.
 *
 * Summary:
 * ValidationResult = specialization of Result for validation failures,
 * with ability to carry multiple errors while still fitting into the Result pipeline.
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