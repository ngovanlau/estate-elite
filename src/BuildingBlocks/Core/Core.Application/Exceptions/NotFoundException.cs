/*
 * Why this NotFoundException is designed this way:
 * ------------------------------------------------
 * 1. Purpose:
 *    - Represents a failure when a requested resource/entity is not found.
 *    - Provides structured information for logging and API responses.
 *
 * 2. Inherits ApplicationException:
 *    - Ensures consistent error code and detail structure across the application.
 *
 * 3. Constructors:
 *    - Entity + key: automatically formats message and adds details for logging.
 *    - Custom message: allows flexibility for non-entity-based not found scenarios.
 *
 * 4. Static Factory Method:
 *    - `For<T>(key)` simplifies creation of entity-specific NotFoundException.
 *    - Improves readability and reduces repetitive code in handlers/services.
 *
 * 5. Details Property:
 *    - Stores structured metadata like entity name and key.
 *    - Useful for logging, diagnostics, or API response payloads.
 *
 * 6. Best Practices:
 *    - Throw NotFoundException when domain/resource cannot be found.
 *    - Use static factory for entity types to standardize messages and details.
 *
 * Overall:
 *    - Provides a clear, type-safe, and structured approach to handle "not found" errors.
 *    - Supports both domain-level clarity and API-layer consistency.
 */

namespace Core.Application.Exceptions;

/// <summary>
/// Exception for when requested resource is not found
/// </summary>
public sealed class NotFoundException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of NotFoundException
    /// </summary>
    /// <param name="name">Entity name</param>
    /// <param name="key">Entity key</param>
    public NotFoundException(string name, object key)
        : base("NotFound.Entity", $"{name} with key '{key}' was not found.")
    {
        WithDetail("EntityName", name).WithDetail("Key", key);
    }

    /// <summary>
    /// Initializes a new instance of NotFoundException with custom message
    /// </summary>
    /// <param name="message">Custom error message</param>
    public NotFoundException(string message)
        : base("NotFound.General", message)
    {
    }

    /// <summary>
    /// Creates NotFoundException for entity
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="key">Entity key</param>
    /// <returns>NotFoundException instance</returns>
    public static NotFoundException For<T>(object key)
    {
        return new NotFoundException(typeof(T).Name, key);
    }
}