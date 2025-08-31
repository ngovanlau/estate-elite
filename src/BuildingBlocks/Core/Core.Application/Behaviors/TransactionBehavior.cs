/*
 * Transaction Behavior (MediatR Pipeline)
 * ---------------------------------------
 * 1. Purpose:
 *    - Ensures all Command handlers run inside a database transaction.
 *    - Automatically handles Commit / Rollback depending on the result.
 *    - Keeps Application Layer clean by abstracting transaction logic away from handlers.
 *
 * 2. How it works:
 *    - The pipeline wraps around the request handler (via MediatR).
 *    - If the request is a **Query** → skip transaction (read-only).
 *    - If the request is a **Command**:
 *        a) BeginTransactionAsync() via IUnitOfWork.
 *        b) Execute the handler.
 *        c) If Result.IsSuccess → CommitTransactionAsync().
 *        d) If Result.Failure or Exception → RollbackTransactionAsync().
 *
 * 3. Benefits:
 *    - Centralized transaction handling (no duplicate try/catch in every handler).
 *    - Guarantees data consistency for Command operations.
 *    - Makes it easy to log transaction lifecycle (start, commit, rollback).
 *    - Queries are unaffected (better performance since no transaction overhead).
 *
 * 4. Example Flow:
 *    - Request: CreateOrderCommand
 *    - Pipeline: TransactionBehavior intercepts
 *    - Begin Transaction
 *    - Call CreateOrderHandler.Handle()
 *    - If success → Commit
 *    - If failure/exception → Rollback
 *
 * 5. Best Practices:
 *    - Keep transaction scope small → avoid long-running operations in handlers.
 *    - Only Commands should change state → hence only Commands are wrapped.
 *    - Log transaction ID for debugging & correlation.
 *    - Combine with UnitOfWork for proper DbContext / database abstraction.
 *
 * 6. Related Patterns:
 *    - Unit of Work (coordinates data access changes).
 *    - Command/Query Responsibility Segregation (CQS/CQRS).
 *    - Pipeline Behaviors (cross-cutting concerns like Logging, Validation, Performance).
 */

using Core.Application.Abstractions.Data;
using Core.Application.Abstractions.Messaging;
using Core.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.Behaviors;

/// <summary>
/// Transaction behavior for MediatR pipeline that wraps commands in database transactions using UnitOfWork
/// </summary>
/// <typeparam name="TRequest">Request type</typeparam>
/// <typeparam name="TResponse">Response type</typeparam>
public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : Result
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IUnitOfWork unitOfWork,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Only wrap Commands in transactions, not Queries
        if (!IsCommand())
        {
            return await next();
        }

        var requestName = typeof(TRequest).Name;
        var transactionId = Guid.NewGuid();

        _logger.LogInformation(
            "Starting transaction {TransactionId} for request {RequestName}",
            transactionId,
            requestName);

        try
        {
            // Begin a transaction via UnitOfWork
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var response = await next();

            if (response.IsSuccess)
            {
                // Commit the transaction if the handler succeeded
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation(
                    "Transaction {TransactionId} for request {RequestName} committed successfully",
                    transactionId,
                    requestName);
            }
            else
            {
                // Rollback if business validation failed
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                _logger.LogWarning(
                    "Transaction {TransactionId} for request {RequestName} rolled back due to business logic failure: {Error}",
                    transactionId,
                    requestName,
                    response.Error);
            }

            return response;
        }
        catch (Exception ex)
        {
            // Rollback if an exception occurs
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            _logger.LogError(ex,
                "Transaction {TransactionId} for request {RequestName} failed and rolled back",
                transactionId,
                requestName);

            throw;
        }
    }

    /// <summary>
    /// Determines if the request is a command that requires a transaction
    /// </summary>
    /// <returns>True if request is a command</returns>
    private static bool IsCommand()
        => typeof(TRequest).Name.EndsWith("Command", StringComparison.OrdinalIgnoreCase) ||
           typeof(TRequest).GetInterfaces().Any(i =>
                (i.IsGenericType &&
                 (i.GetGenericTypeDefinition() == typeof(IRequest<>) ||
                  i.GetGenericTypeDefinition() == typeof(ICommand<>)))
                || i == typeof(ICommand));
}
