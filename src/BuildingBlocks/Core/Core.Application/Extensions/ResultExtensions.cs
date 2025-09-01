/* 
 * Why this ResultExtensions class exists:
 * --------------------------------------
 * 1. Purpose:
 *    - Bridges the Result pattern (domain/application layer) with ASP.NET Core IActionResult (API layer).
 *    - Ensures controllers can return standardized HTTP responses without duplicating error-handling logic.
 *    - Keeps controllers clean → they only care about Result, not HTTP specifics.
 *
 * 2. Handling Success:
 *    - Result<T>.IsSuccess:
 *        * If Value is null → return 204 NoContent.
 *        * If Value exists → return 200 OK with Value as payload.
 *    - Result (non-generic):
 *        * Always 204 NoContent on success.
 *    - Provides additional helpers:
 *        * ToCreatedResult → 201 Created with Location header.
 *        * ToCreatedAtActionResult → 201 Created with route binding to action/controller.
 *
 * 3. Handling Errors:
 *    - Maps ErrorType to proper HTTP status codes:
 *        * Validation → 400 Bad Request
 *        * NotFound   → 404 Not Found
 *        * Conflict   → 409 Conflict
 *        * Unauthorized → 401 Unauthorized
 *        * Forbidden  → 403 Forbidden
 *        * Default    → 500 Internal Server Error
 *
 * 4. ProblemDetails Integration:
 *    - Uses ProblemDetails (RFC 7807) for consistent error contracts.
 *    - For Validation errors with details → uses ValidationProblemDetails.
 *      * Includes structured error dictionary { property → [messages[]] }.
 *    - For other errors → generic ProblemDetails with optional details in Extensions.
 *
 * 5. Benefits:
 *    - Controllers become thin and declarative:
 *        return result.ToActionResult();
 *    - Ensures consistent error responses across the API.
 *    - Improves client experience with structured error payloads.
 *    - Reduces boilerplate code (no need to manually map Result to HTTP response).
 *
 * Overall:
 * ResultExtensions enforces a clean separation of concerns: 
 * the domain communicates outcome via Result/Error, and the web layer translates it into proper HTTP responses.
 */

using System.Net;
using Core.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Core.Application.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Converts a Result to an IActionResult.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result to create a response for.</param>
    /// <returns>An IActionResult representing the result.</returns>
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return result.Value is null ? new NoContentResult() : new OkObjectResult(result.Value);

        return CreateErrorResponse(result.Error!);
    }

    /// <summary>
    /// Converts a Result to an IActionResult.
    /// </summary>
    /// <param name="result">The result to create a response for.</param>
    /// <returns>An IActionResult representing the result.</returns>
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new NoContentResult();

        return CreateErrorResponse(result.Error!);
    }

    /// <summary>
    /// Creates a no content result response.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result to create a response for.</param>
    /// <param name="location">The location of the created resource.</param>
    /// <returns>An IActionResult representing the created result response.</returns>
    public static IActionResult ToCreatedResult<T>(this Result<T> result, string location)
    {
        if (result.IsSuccess)
            return new CreatedResult(location, result.Value);

        return CreateErrorResponse(result.Error!);
    }

    /// <summary>
    /// Creates a created result response.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result to create a response for.</param>
    /// <param name="actionName">The name of the action that created the resource.</param>
    /// <param name="controllerName">The name of the controller that created the resource.</param>
    /// <param name="routeValues">The route values for the created resource.</param>
    /// <returns>An IActionResult representing the created result response.</returns>
    public static IActionResult ToCreatedAtActionResult<T>(
        this Result<T> result,
        string actionName,
        string? controllerName = null,
        object? routeValues = null)
    {
        if (result.IsSuccess)
            return new CreatedAtActionResult(actionName, controllerName, routeValues, result.Value);

        return CreateErrorResponse(result.Error!);
    }

    /// <summary>
    /// Creates an error response based on the provided error.
    /// </summary>
    /// <param name="error">The error to create a response for.</param>
    /// <returns>An IActionResult representing the error response.</returns>
    private static ObjectResult CreateErrorResponse(Error error)
    {
        var statusCode = GetStatusCode(error.Type);

        // If the error is a validation error and has details, return a ValidationProblemDetails
        if (error.Type == ErrorType.Validation && error.Details is { Count: > 0 })
        {
            var validationProblem = new ValidationProblemDetails
            {
                Title = "Validation Failed",
                Status = statusCode,
                Detail = error.Message,
                Instance = error.Code
            };

            foreach (var kv in error.Details)
            {
                validationProblem.Errors.Add(kv.Key, [kv.Value.ToString()!]);
            }

            return new ObjectResult(validationProblem) { StatusCode = statusCode };
        }

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(error.Type),
            Status = statusCode,
            Detail = error.Message,
            Instance = error.Code
        };

        if (error.Details != null && error.Details.Count > 0)
            problemDetails.Extensions.Add("details", error.Details);

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => (int)HttpStatusCode.BadRequest,
        ErrorType.NotFound => (int)HttpStatusCode.NotFound,
        ErrorType.Conflict => (int)HttpStatusCode.Conflict,
        ErrorType.Unauthorized => (int)HttpStatusCode.Unauthorized,
        ErrorType.Forbidden => (int)HttpStatusCode.Forbidden,
        _ => (int)HttpStatusCode.InternalServerError
    };

    private static string GetTitle(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Bad Request",
        ErrorType.NotFound => "Not Found",
        ErrorType.Conflict => "Conflict",
        ErrorType.Unauthorized => "Unauthorized",
        ErrorType.Forbidden => "Forbidden",
        _ => "Internal Server Error"
    };
}
