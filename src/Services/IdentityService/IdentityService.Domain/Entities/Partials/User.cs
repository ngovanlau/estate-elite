namespace IdentityService.Domain.Entities;

public partial class User
{
    public User() { }

    public static User Create(string username, string email, string fullname, string passwordHash)
    {
        return new User
        {
            Username = username,
            Email = email,
            Fullname = fullname,
            PasswordHash = passwordHash
        };
    }
}
