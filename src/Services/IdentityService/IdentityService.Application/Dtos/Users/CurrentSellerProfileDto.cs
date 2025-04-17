namespace IdentityService.Application.Dtos.Users;

public class CurrentSellerProfileDto
{
    public required string CompanyName { get; set; }
    public string? LicenseNumber { get; set; }
    public required string TaxIdentificationNumber { get; set; }
    public string? ProfessionalLicense { get; set; }
    public string? Biography { get; set; }
    public bool IsVerified { get; set; }
}
