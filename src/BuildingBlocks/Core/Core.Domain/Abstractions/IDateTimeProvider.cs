/*
 * Why use IDateTimeProvider instead of DateTime.UtcNow directly:
 * -------------------------------------------------------------
 * 1. Abstraction over System Clock:
 *    - Avoids hard dependency on static DateTime.UtcNow.
 *    - Makes code more testable (can inject a fake or fixed time in unit tests).
 *
 * 2. Consistency:
 *    - Centralizes how current time is retrieved.
 *    - All parts of the system use the same time source (e.g., UTC only).
 *
 * 3. Flexibility:
 *    - Allows changing the underlying implementation later (e.g., sync with NTP server,
 *      time-zone adjustments, or using a distributed clock in microservices).
 *
 * 4. Clean Architecture principle:
 *    - Infrastructure provides the actual implementation.
 *    - Domain/application code depends only on the abstraction (IDateTimeProvider).
 *
 * Example:
 *    public class SystemDateTimeProvider : IDateTimeProvider
 *    {
 *        public DateTime UtcNow => DateTime.UtcNow;
 *    }
 *
 * Overall:
 * IDateTimeProvider improves testability, consistency, and maintainability by
 * abstracting away direct usage of system time.
 */

namespace Core.Domain.Abstractions;

/// <summary>
/// Interface for providing date and time information.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTime UtcNow { get; }
}