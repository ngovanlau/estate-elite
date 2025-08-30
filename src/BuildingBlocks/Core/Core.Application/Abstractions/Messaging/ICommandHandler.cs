/*
 * Why define ICommandHandler and ICommandHandler<TCommand, TResponse>:
 * -------------------------------------------------------------------
 * 1. Command Handling in CQRS:
 *    - Each command has a handler responsible for executing its business logic.
 *    - Handlers encapsulate the action and any side effects.
 *
 * 2. MediatR Integration:
 *    - ICommandHandler<TCommand> extends IRequestHandler<TCommand, Result>.
 *    - ICommandHandler<TCommand, TResponse> extends IRequestHandler<TCommand, Result<TResponse>>.
 *    - Allows commands to be processed via MediatR pipeline.
 *
 * 3. Result Pattern:
 *    - Handlers return Result or Result<TResponse> to indicate success/failure.
 *    - Avoids throwing exceptions for expected business failures.
 *    - Makes success/failure explicit and type-safe.
 *
 * 4. Type Safety:
 *    - ICommandHandler<TCommand> → commands with no return value.
 *    - ICommandHandler<TCommand, TResponse> → commands that return a typed response.
 *
 * 5. Benefits:
 *    - Provides a consistent contract for command handlers.
 *    - Simplifies error handling and chaining operations.
 *    - Supports functional composition with Result<T>.
 *
 * Overall:
 * These abstractions enforce a consistent, testable, and type-safe way to
 * handle commands in the application layer, aligning with CQRS and Clean Architecture principles.
 */

using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Abstractions.Messaging;

/// <summary>
/// Represents a command handler that returns no value
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

/// <summary>
/// Represents a command handler that returns a value
/// </summary>
/// <typeparam name="TCommand">The command type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}