using Common.Application.Responses;
using MediatR;

namespace IdentityService.Application.Requests.Users;

public class UpdateSellerProfileRequest : IRequest<ApiResponse>
{
    public required string CompanyName { get; set; }
    public string? LicenseNumber { get; set; }
    public required string TaxIdentificationNumber { get; set; }
    public string? ProfessionalLicense { get; set; }
    public string? Biography { get; set; }
    public int EstablishedYear { get; set; }
    public bool AcceptsPaypal { get; set; }
    public string? PaypalEmail { get; set; }
    public string? PaypalMerchantId { get; set; }
}
