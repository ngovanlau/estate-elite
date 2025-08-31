/*
 * ValidationBehavior in MediatR Pipeline
 * --------------------------------------
 * 1. Purpose:
 *    - Runs before the actual handler executes.
 *    - Uses FluentValidation validators (IValidator<TRequest>) to check the request.
 *    - Converts FluentValidation failures into our custom Result/ValidationResult pattern.
 *
 * 2. Workflow:
 *    - If no validators exist for the request → continue pipeline.
 *    - If validators exist:
 *        • Collect validation errors (Error[]).
 *        • If errors found → return ValidationResult or ValidationResult<T>.
 *        • If no errors → call next() (execute handler).
 *
 * 3. Key Points:
 *    - The behavior enforces validation across all requests without duplicating code.
 *    - Uses CreateValidationResult<TResponse> helper:
 *        • If TResponse = Result      → return ValidationResult.
 *        • If TResponse = Result<T>   → return ValidationResult<T>.
 *
 * 4. Integration:
 *    - Registered automatically in DI with:
 *          services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
 *    - Ensures every Command/Query passes through validation before execution.
 *
 * 5. Benefits:
 *    - Centralized validation logic.
 *    - Clean error mapping (FluentValidation → Domain Error).
 *    - Works seamlessly with Result pattern, so handlers don't need to worry about validation.
 *
 * Example:
 * --------
 * var result = await mediator.Send(new CreateUserCommand(...));
 * if (result.IsFailure && result is IValidationResult v)
 * {
 *     // Handle validation errors
 *     foreach (var error in v.Errors)
 *         Console.WriteLine($"{error.Code}: {error.Message}");
 * }
 */

using Core.Domain.Shared;
using FluentValidation;
using MediatR;

namespace Core.Domain.Application.Behaviors;

/// <summary>
/// Validation behavior for MediatR pipeline
/// </summary>
/// <typeparam name="TRequest">Request type</typeparam>
/// <typeparam name="TResponse">Response type</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initializes a new instance of ValidationBehavior
    /// </summary>
    /// <param name="validators">Collection of validators</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        Error[] errors = _validators
            .Select(validator => validator.ValidateAsync(request))
            .SelectMany(validationResult => validationResult.Result.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => Error.Validation(
                failure.PropertyName,
                failure.ErrorMessage))
            .ToArray();

        if (errors.Length > 0)
            return CreateValidationResult<TResponse>(errors);

        return await next();
    }

    /// <summary>
    /// Creates validation result based on response type
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="errors">Validation errors</param>
    /// <returns>Validation result</returns>
    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
            return (ValidationResult.WithErrors(errors) as TResult)!;

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, [errors])!;

        return (TResult)validationResult;
    }
}