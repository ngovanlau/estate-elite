/*
 * FilteringParameters
 * -------------------
 * 1. Purpose:
 *    - Encapsulates filtering & searching options when querying data.
 *    - Provides a consistent structure for passing filters from API → Application layer.
 *
 * 2. Properties:
 *    - SearchTerm (string?):
 *        General keyword used for full-text or fuzzy search.
 *        Example: "john" → search by name/email/username.
 *
 *    - Filters (Dictionary<string, string>?):
 *        Flexible key-value filters for structured filtering.
 *        Example:
 *          {
 *            "role": "admin",
 *            "status": "active"
 *          }
 *
 * 3. Usage:
 *    - Commonly combined with PagingParameters / SortingParameters.
 *    - Used in Queries or API endpoints to allow dynamic filtering.
 *
 *    Example:
 *      var query = dbContext.Users.AsQueryable();
 *
 *      if (!string.IsNullOrWhiteSpace(filtering.SearchTerm))
 *          query = query.Where(u => u.Name.Contains(filtering.SearchTerm));
 *
 *      if (filtering.Filters?.ContainsKey("role") == true)
 *          query = query.Where(u => u.Role == filtering.Filters["role"]);
 *
 *    var result = await PagedList<User>.CreateAsync(query, page, size);
 *
 * 4. Best Practices:
 *    - Keep this class immutable in domain layer, but mutable in Application layer is acceptable.
 *    - Normalize keys in Filters (e.g., lowercase) for consistent usage.
 *    - Validate known filters at API layer (avoid SQL injection / invalid keys).
 *
 * 5. Related:
 *    - PagingParameters (page/size)
 *    - SortingParameters (order by)
 *    - Often grouped into a single "QueryParameters" DTO for API requests.
 */

namespace Core.Application.Models;

/// <summary>
/// Parameters for filtering
/// </summary>
public class FilteringParameters
{
    /// <summary>
    /// General search term
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Additional filters
    /// </summary>
    public Dictionary<string, string>? Filters { get; set; }
}
