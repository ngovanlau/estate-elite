/* 
 * Why this ValidationBehavior is designed this way:
 * ------------------------------------------------
 * 1. Purpose (Pipeline Behavior):
 *    - Implements MediatR IPipelineBehavior<TRequest, TResponse>.
 *    - Intercepts every request before reaching the handler.
 *    - Ensures validation runs automatically, enforcing consistency across all requests.
 *
 * 2. FluentValidation Integration:
 *    - Injects IEnumerable<IValidator<TRequest>> to support multiple validators per request.
 *    - Each validator can define its own rules → composable and extensible.
 *    - Validation runs asynchronously to allow async rules (e.g., DB checks).
 *
 * 3. Validation Result Handling:
 *    - Collects all validation errors instead of stopping at the first failure.
 *    - Groups errors by PropertyName, aggregating messages into a dictionary.
 *    - Provides structured details { PropertyName → [ErrorMessages[]] } for clients.
 *
 * 4. Error Handling via Result Pattern:
 *    - Does not throw exceptions for validation errors → avoids control-flow exceptions.
 *    - Wraps failures in an Error object (Error.Validation).
 *    - Returns Result or Result<T> consistently, keeping the functional style.
 *
 * 5. Generic Result Support:
 *    - Supports both Result and Result<T>.
 *    - Uses reflection only when needed to call Result<T>.Failure(error).
 *    - This ensures pipeline can handle any handler response type uniformly.
 *
 * 6. Sealed Class:
 *    - Prevents inheritance → no accidental override of validation logic.
 *    - Ensures single, consistent validation flow in the application.
 *
 * 7. Benefits:
 *    - Centralizes validation logic (no duplication across handlers).
 *    - Provides rich error feedback for API consumers.
 *    - Follows Clean Architecture & DDD principles:
 *      * Separation of concerns (validation outside handlers).
 *      * Uniform error contract (Result/Error types).
 *      * Extensible without modifying core pipeline.
 *
 * Overall:
 * ValidationBehavior guarantees that all requests are validated consistently,
 * errors are structured and returned via the Result pattern, and business logic
 * in handlers remains focused on actual use cases without worrying about validation.
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

        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(request, cancellationToken)));

        var details = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(failure => failure.ErrorMessage).ToArray() as object);

        var error = Error.Validation("ValidationError", "Validation failed", details);

        if (details.Count > 0)
            return CreateValidationResult<TResponse>(error);

        return await next();
    }

    /// <summary>
    /// Creates validation result based on response type
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="error">Validation error</param>
    /// <returns>Validation result</returns>
    private static TResult CreateValidationResult<TResult>(Error error)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
            return (Result.Failure(error) as TResult)!;

        object validationResult = typeof(Result<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(Result.Failure))!
            .Invoke(null, [error])!;

        return (TResult)validationResult;
    }
}
