namespace IdentityService.Application.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Fullname { get; set; }
}
