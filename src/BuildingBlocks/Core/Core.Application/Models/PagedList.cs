/*
 * PagedList<T>
 * ------------
 * 1. Purpose:
 *    - Represents a single "page" of results when querying large datasets.
 *    - Encapsulates both the paginated items and the paging metadata 
 *      (current page, total pages, etc.).
 *    - Works directly with IQueryable<T> (e.g., EF Core LINQ queries).
 *
 * 2. How it works:
 *    - Accepts a collection of items (current page) + total item count.
 *    - Calculates paging properties:
 *        TotalPages      = ceil(TotalItems / PageSize)
 *        HasNextPage     = PageNumber < TotalPages
 *        HasPreviousPage = PageNumber > 1
 *
 * 3. Key Features:
 *    - Immutable: Items are exposed as IReadOnlyList<T>.
 *    - Async factory `CreateAsync`: executes COUNT + paginated query in EF Core.
 *    - `Empty()`: convenience method for returning an empty page.
 *    - Prevents repeating paging logic in every query handler.
 *
 * 4. Typical Usage:
 *    var query = dbContext.Users.Where(u => u.IsActive);
 *    var pagedUsers = await PagedList<User>.CreateAsync(query, pageNumber, pageSize);
 *
 *    Console.WriteLine($"Page {pagedUsers.PageNumber}/{pagedUsers.TotalPages}");
 *    foreach (var user in pagedUsers.Items)
 *    {
 *        Console.WriteLine(user.Name);
 *    }
 *
 * 5. Best Practices:
 *    - Always use IQueryable<T> as input (so EF can translate Skip/Take into SQL).
 *    - Keep pageSize reasonable (avoid fetching thousands of rows at once).
 *    - Consider enforcing maxPageSize at API layer for performance & safety.
 *    - Return PagedList<T> in Queries (not Commands).
 *
 * 6. Related Patterns:
 *    - Query Object (encapsulates complex queries).
 *    - Repository Pattern (PagedList can be a return type).
 *    - API Pagination (pairs with DTOs for frontend).
 */

using Microsoft.EntityFrameworkCore;

namespace Core.Application.Models;

public class PagedList<T>
{
    /// <summary>
    /// Items in the current page
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public int TotalItems { get; }

    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Size of each page
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Whether there is a next page
    /// </summary>
    public bool HasNextPage { get; }

    /// <summary>
    /// Whether there is a previous page
    /// </summary>
    public bool HasPreviousPage { get; }

    /// <summary>
    /// Initializes a new instance of PagedList
    /// </summary>
    /// <param name="items">Items in current page</param>
    /// <param name="totalItems">Total number of items across all pages</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Size of each page</param>
    public PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items.ToList().AsReadOnly();
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        HasNextPage = PageNumber < TotalPages;
        HasPreviousPage = PageNumber > 1;
    }

    /// <summary>
    /// Creates an empty paged list
    /// </summary>
    /// <returns>Empty paged list</returns>
    public static PagedList<T> Empty() => new([], 0, 1, 10);

    /// <summary>
    /// Creates a paged list from a queryable source
    /// </summary>
    /// <param name="source">Source queryable</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list</returns>
    public static async Task<PagedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalItems = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>(items, totalItems, pageNumber, pageSize);
    }
}