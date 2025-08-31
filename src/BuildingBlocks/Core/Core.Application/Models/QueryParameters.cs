/*
 * QueryParameters
 * ---------------
 * 1. Purpose:
 *    - Encapsulates all query-related options: pagination, sorting, and filtering.
 *    - Provides a single DTO for API endpoints or query handlers to handle user input consistently.
 *
 * 2. Properties:
 *    - Pagination (PaginationParameters):
 *        PageNumber, PageSize, SearchTerm
 *
 *    - Sorting (SortingParameters):
 *        Field to sort by, direction (ascending/descending)
 *
 *    - Filtering (FilteringParameters):
 *        SearchTerm (general keyword) and key-value Filters for structured filtering
 *
 * 3. Usage:
 *    - Typically passed from API controller to application layer.
 *    - Example:
 *        var query = dbContext.Users.AsQueryable();
 *
 *        // Filtering
 *        if (!string.IsNullOrWhiteSpace(queryParameters.Filtering.SearchTerm))
 *            query = query.Where(u => u.Name.Contains(queryParameters.Filtering.SearchTerm));
 *
 *        foreach (var kv in queryParameters.Filtering.Filters ?? new Dictionary<string, string>())
 *        {
 *            if (kv.Key == "role")
 *                query = query.Where(u => u.Role == kv.Value);
 *        }
 *
 *        // Sorting
 *        query = queryParameters.Sorting.Direction == "desc"
 *            ? query.OrderByDescending(u => EF.Property<object>(u, queryParameters.Sorting.SortBy))
 *            : query.OrderBy(u => EF.Property<object>(u, queryParameters.Sorting.SortBy));
 *
 *        // Pagination
 *        var pagedResult = await PagedList<User>.CreateAsync(
 *            query,
 *            queryParameters.Pagination.PageNumber,
 *            queryParameters.Pagination.PageSize);
 *
 * 4. Best Practices:
 *    - Normalize filter keys and validate at API layer.
 *    - Enforce reasonable max page size.
 *    - Keep sorting fields validated to avoid invalid SQL / runtime errors.
 *    - Use this DTO as standard input for query handlers.
 *
 * 5. Related:
 *    - PaginationParameters
 *    - FilteringParameters
 *    - SortingParameters
 *    - PagedList<T>
 */

namespace Core.Application.Models;

/// <summary>
/// Combined query parameters (pagination + sorting + filtering)
/// </summary>
public class QueryParameters
{
    /// <summary>
    /// Pagination parameters
    /// </summary>
    public PaginationParameters Pagination { get; set; } = new();

    /// <summary>
    /// Sorting parameters
    /// </summary>
    public SortingParameters Sorting { get; set; } = new();

    /// <summary>
    /// Filtering parameters
    /// </summary>
    public FilteringParameters Filtering { get; set; } = new();
}
