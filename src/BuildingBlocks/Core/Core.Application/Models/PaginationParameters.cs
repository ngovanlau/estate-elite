/*
 * PaginationParameters
 * --------------------
 * 1. Purpose:
 *    - Encapsulates paging info for queries (page number & page size).
 *    - Ensures consistent handling of pagination across API / Application layers.
 *
 * 2. Properties:
 *    - PageNumber (int):
 *        1-based current page number (default: 1).
 *
 *    - PageSize (int):
 *        Number of items per page (default: 10, max: 100).
 *        Private backing field enforces max size to prevent performance issues.
 *
 *    - SearchTerm (string?):
 *        Optional general search term.
 *        Can be combined with FilteringParameters / SortingParameters.
 *
 * 3. Usage:
 *    - Often used in queries with PagedList<T>.CreateAsync.
 *
 *      Example:
 *        var query = dbContext.Users.AsQueryable();
 *
 *        if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
 *            query = query.Where(u => u.Name.Contains(pagination.SearchTerm));
 *
 *        var pagedUsers = await PagedList<User>.CreateAsync(query, pagination.PageNumber, pagination.PageSize);
 *
 * 4. Best Practices:
 *    - Keep max page size reasonable to avoid performance bottlenecks.
 *    - Normalize PageNumber >= 1.
 *    - Combine with filtering & sorting DTOs for API endpoints.
 *
 * 5. Related:
 *    - FilteringParameters (filters/search)
 *    - SortingParameters (order by)
 *    - PagedList<T> (returns paginated results)
 */

namespace Core.Application.Models;

/// <summary>
/// Parameters for pagination
/// </summary>
public class PaginationParameters
{
    /// <summary>
    /// Maximum page size
    /// </summary>
    private const int MaxPageSize = 100;

    /// <summary>
    /// Page size (default: 10, max: 100)
    /// </summary>
    private int _pageSize = 10;

    /// <summary>
    /// Page number (1-based, default: 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size (default: 10, max: 100)
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    /// <summary>
    /// Search term for filtering
    /// </summary>
    public string? SearchTerm { get; set; }
}
