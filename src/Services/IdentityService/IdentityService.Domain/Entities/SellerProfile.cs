namespace IdentityService.Domain.Entities;

public partial class SellerProfile
{
    public Guid UserId { get; set; }
    public required User User { get; set; }

    // Professional Information
    public required string CompanyName { get; set; }
    public string? LicenseNumber { get; set; }
    public required string TaxIdentificationNumber { get; set; }
    public string? ProfessionalLicense { get; set; }

    // Professional Details
    public string? Biography { get; set; }

    public int EstablishedYear { get; set; }

    // Verification Status
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; }

    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime? ModifiedOn { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool IsDelete { get; set; }
}