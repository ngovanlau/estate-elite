/*
 * Why define Result this way:
 * ---------------------------
 * 1. Purpose of Result:
 *    - Encapsulates the outcome of an operation.
 *    - Instead of throwing exceptions for business failures,
 *      we return Result which indicates Success or Failure.
 *    - Improves readability, reduces exception misuse, and makes flow explicit.
 *
 * 2. Result (without value):
 *    - Contains:
 *        - IsSuccess (true/false).
 *        - Error (Error.None if success).
 *    - Static factories: Success(), Failure(Error), Create(condition, error).
 *    - Prevents invalid states:
 *        - Success cannot contain Error.
 *        - Failure must contain an Error.
 *
 * 3. Result<TValue> (with value):
 *    - Extends Result to carry a success value.
 *    - Value property:
 *        - Accessible only if IsSuccess is true.
 *        - Throws if accessed on failure → ensures correctness.
 *    - Implicit conversion from TValue → Result<TValue>:
 *        - Simplifies usage (can return raw value, automatically wrapped).
 *    - Functional methods:
 *        - Map(): transform success value into another type.
 *        - Bind(): chain operations that also return Result.
 *        - Enables fluent pipelines without nested if-checks.
 *
 * 4. Error Handling:
 *    - Error type (custom) centralizes domain/application errors.
 *    - Makes failures explicit and predictable.
 *
 * 5. DDD / Clean Architecture Principle:
 *    - Application services can always return Result instead of throwing.
 *    - Domain logic can express both valid (success) and invalid (failure) outcomes clearly.
 *
 * Overall:
 * This Result pattern enforces explicit handling of success/failure,
 * avoids exception-driven control flow, and enables functional-style composition.
 */

namespace Core.Domain.Shared;

/// <summary>
/// Result pattern implement for operation result
/// </summary>
public class Result
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Error information (None if successful)
    /// </summary>
    public Error Error { get; } = Error.None;

    /// <summary>
    /// Protected constructor to ensure proper instantiation
    /// </summary>
    /// <param name="isSuccess">Whether the operation was successful</param>
    /// <param name="error">Error information if operation failed</param>
    /// <exception cref="InvalidOperationException">Thrown when success/error combination is invalid</exception>
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Successfully result cannot have error");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Failed result must have an error");

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    /// <returns>Successful result</returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Creates a successful result with a value
    /// </summary>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <param name="value">The success value</param>
    /// <returns>Successful result with value</returns>
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    /// <summary>
    /// Creates a failed result
    /// </summary>
    /// <param name="error">Error information</param>
    /// <returns>Failed result</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Creates a failed result with a specific value type
    /// </summary>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <param name="error">Error information</param>
    /// <returns>Failed result</returns>
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    /// <summary>
    /// Creates a result based on a nullable value
    /// </summary>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <param name="value">The value (null = failure)</param>
    /// <returns>Success if value is not null, failure otherwise</returns>
    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    /// <summary>
    /// Creates a result based on a condition
    /// </summary>
    /// <param name="condition">Condition to evaluate</param>
    /// <param name="error">Error if condition is false</param>
    /// <returns>Success if condition is true, failure otherwise</returns>
    public static Result Create(bool condition, Error error) =>
        condition ? Success() : Failure(error);
}

/// <summary>
/// Result pattern implementation with a value
/// </summary>
/// <typeparam name="TValue">Type of the success value</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// Constructor for result with value
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="isSuccess">Whether operation was successful</param>
    /// <param name="error">Error information</param>
    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// The success value (throws if result is failure)
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when accessing value of failed result</exception>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    /// <summary>
    /// Implicit conversion from value to result
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns>Success result if value is not null</returns>
    public static implicit operator Result<TValue>(TValue? value) => Create(value);

    /// <summary>
    /// Maps the result value to another type
    /// </summary>
    /// <typeparam name="TNew">Target type</typeparam>
    /// <param name="mapper">Mapping function</param>
    /// <returns>Mapped result</returns>
    public Result<TNew> Map<TNew>(Func<TValue, TNew> mapper)
    {
        return IsSuccess ? Success(mapper(Value)) : Failure<TNew>(Error);
    }

    /// <summary>
    /// Binds the result to another operation
    /// </summary>
    /// <typeparam name="TNew">Target type</typeparam>
    /// <param name="binder">Binding function</param>
    /// <returns>Bound result</returns>
    public Result<TNew> Bind<TNew>(Func<TValue, Result<TNew>> binder)
    {
        return IsSuccess ? binder(Value) : Failure<TNew>(Error);
    }
}