using SharedKernel.Enums;

namespace IdentityService.Domain.Entities;

public partial class User
{
    public static User Create(string username, string email, string fullname, string passwordHash, UserRole role = UserRole.Buyer)
    {
        return new User
        {
            Username = username,
            Email = email,
            Fullname = fullname,
            PasswordHash = passwordHash,
            Role = role,
        };
    }
}
