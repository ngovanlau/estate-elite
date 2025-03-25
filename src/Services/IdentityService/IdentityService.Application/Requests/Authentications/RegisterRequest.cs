namespace IdentityService.Application.Requests.Authentications;

public class RegisterRequest
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Fullname { get; set; }
    public string? Password { get; set; }
}
