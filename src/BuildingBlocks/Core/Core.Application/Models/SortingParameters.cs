/*
 * SortingParameters
 * -----------------
 * 1. Purpose:
 *    - Encapsulates sorting options for queries or API requests.
 *    - Provides a consistent way to specify which field to sort by and in which direction.
 *
 * 2. Properties:
 *    - SortBy (string?):
 *        The name of the property or column to sort by.
 *        Example: "Name", "CreatedOn", "Price"
 *
 *    - SortDirection (string?):
 *        Direction of sorting. Typically "asc" (ascending) or "desc" (descending).
 *        Default is "asc".
 *
 *    - IsSortDescending (bool):
 *        Convenience property that returns true if SortDirection is "desc" (case-insensitive).
 *
 * 3. Usage:
 *    - Used together with PagingParameters and FilteringParameters in queries.
 *    - Example:
 *        var query = dbContext.Users.AsQueryable();
 *
 *        if (!string.IsNullOrEmpty(sorting.SortBy))
 *        {
 *            query = sorting.IsSortDescending
 *                ? query.OrderByDescending(u => EF.Property<object>(u, sorting.SortBy))
 *                : query.OrderBy(u => EF.Property<object>(u, sorting.SortBy));
 *        }
 *
 * 4. Best Practices:
 *    - Validate SortBy against allowed properties to prevent invalid queries or SQL injection.
 *    - Use default sorting if SortBy is null or empty.
 *    - Keep SortDirection normalized ("asc"/"desc") for consistent behavior.
 *
 * 5. Related:
 *    - PaginationParameters (for paging)
 *    - FilteringParameters (for filtering)
 *    - QueryParameters (combines sorting, filtering, and pagination)
 */

namespace Core.Application.Models;

/// <summary>
/// Parameters for sorting
/// </summary>
public class SortingParameters
{
    /// <summary>
    /// Sort by field
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction (asc/desc)
    /// </summary>
    public string? SortDirection { get; set; } = "asc";

    /// <summary>
    /// Whether sorting is descending
    /// </summary>
    public bool IsSortDescending =>
        !string.IsNullOrEmpty(SortDirection) &&
        SortDirection.Equals("desc", StringComparison.InvariantCultureIgnoreCase);
}
