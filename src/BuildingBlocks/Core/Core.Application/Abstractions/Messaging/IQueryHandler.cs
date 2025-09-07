/*
 * Why define IQueryHandler<TQuery, TResponse>:
 * -------------------------------------------
 * 1. Query Handling in CQRS:
 *    - Each query has a handler responsible for retrieving data.
 *    - Handlers encapsulate the read logic, separate from commands (writes).
 *
 * 2. MediatR Integration:
 *    - Extends IRequestHandler<TQuery, Result<TResponse>>.
 *    - Queries are processed via MediatR pipeline for decoupled dispatching.
 *
 * 3. Result Pattern:
 *    - Handlers return Result<TResponse> to indicate success/failure explicitly.
 *    - Provides structured errors (e.g., NotFound, Validation).
 *    - Avoids exceptions for expected failures.
 *
 * 4. Type Safety:
 *    - TQuery defines the query input type.
 *    - TResponse defines the expected result type.
 *
 * 5. Benefits:
 *    - Consistent, testable query handling across the application layer.
 *    - Clear separation between read and write operations.
 *    - Supports functional-style composition using Map/Bind on Result<T>.
 *
 * Overall:
 * IQueryHandler<TQuery, TResponse> enforces a clean, type-safe, and
 * predictable way to handle queries following CQRS and Clean Architecture principles.
 */

using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Abstractions.Messaging;

/// <summary>
/// Represents a query handler
/// </summary>
/// <typeparam name="TQuery">The query type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
