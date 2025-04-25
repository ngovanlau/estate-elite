namespace PaymentService.Application.Dtos;

public class SellerDto
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string? PayPalEmail { get; set; }
    public string? PayPalMerchantId { get; set; }
    public bool AcceptsPayPal { get; set; }
}
