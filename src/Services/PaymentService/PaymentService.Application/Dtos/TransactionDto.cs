using SharedKernel.Enums;

namespace PaymentService.Application.Dtos;

public class TransactionDto
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public TransactionStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime CreatedOn { get; set; }
}
