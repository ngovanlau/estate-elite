namespace Core.Application.Abstractions.Services;

/// <summary>
/// Service for getting current user information
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Current user ID
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Current user email
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Current user roles
    /// </summary>
    IReadOnlyList<string> Roles { get; }

    /// <summary>
    /// Whether user is authenticated
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Checks if user has specific role
    /// </summary>
    /// <param name="role">Role to check</param>
    /// <returns>True if user has role</returns>
    bool IsInRole(string role);
}