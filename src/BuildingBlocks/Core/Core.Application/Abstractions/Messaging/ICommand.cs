/*
 * Why define ICommand and ICommand<TResponse> this way:
 * -----------------------------------------------------
 * 1. CQRS Principle:
 *    - Commands represent actions that **change state** in the system.
 *    - They encapsulate intent, not data queries.
 *
 * 2. MediatR Integration:
 *    - ICommand extends IRequest<Result> (or Result<TResponse>).
 *    - Allows commands to be sent via MediatR pipeline.
 *    - Handlers process commands and return a Result/Result<T>.
 *
 * 3. Result Pattern:
 *    - Ensures all commands return success/failure explicitly.
 *    - No exceptions are thrown for expected business failures.
 *    - Result<TResponse> allows returning a value (e.g., newly created entity ID).
 *
 * 4. Type Safety:
 *    - ICommand → commands that do not return a value.
 *    - ICommand<TResponse> → commands that return a typed value.
 *
 * 5. Benefits:
 *    - Consistent handling of success/failure across application layer.
 *    - Works well with validation, domain events, and transaction pipelines.
 *    - Decouples command definition from infrastructure and business logic.
 *
 * Overall:
 * These abstractions provide a clean, testable, and consistent contract for
 * application commands following CQRS and Clean Architecture principles.
 */

using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Abstractions.Messaging;

/// <summary>
/// Represents a command that returns no value
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Represents a command that returns a value.
/// </summary>
/// <typeparam name="TResponse">The response type</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
