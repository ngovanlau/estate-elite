using Common.Domain.Enums;

namespace Common.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? Id { get; }
    string? Email { get; }
    string? Username { get; }
    UserRole? Role { get; }
    string GetClaimValue(string claimType);
    List<string> GetClaimValues(string claimType);
}
