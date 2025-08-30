/*
 * Why define IQuery<TResponse>:
 * -----------------------------
 * 1. CQRS Principle:
 *    - Queries represent **read-only operations** that retrieve data.
 *    - They do not modify the state of the system.
 *
 * 2. MediatR Integration:
 *    - IQuery<TResponse> extends IRequest<Result<TResponse>>.
 *    - Allows queries to be sent via MediatR pipeline and handled by query handlers.
 *
 * 3. Result Pattern:
 *    - Ensures queries return success/failure explicitly.
 *    - Encapsulates errors (e.g., NotFound, Validation, Conflict) in a structured way.
 *    - Avoids throwing exceptions for expected failures.
 *
 * 4. Type Safety:
 *    - TResponse defines the expected return type of the query.
 *    - Consumers can rely on Result<TResponse> to safely access data if successful.
 *
 * 5. Benefits:
 *    - Clear separation of read operations from write operations (CQRS).
 *    - Consistent error handling across queries.
 *    - Supports functional composition with Map/Bind on Result<TResponse>.
 *
 * Overall:
 * IQuery<TResponse> provides a clean, testable, and consistent contract for
 * application queries following CQRS and Clean Architecture principles.
 */

using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Abstractions.Messaging;

/// <summary>
/// Represents a query
/// </summary>
/// <typeparam name="TResponse">The response type</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}