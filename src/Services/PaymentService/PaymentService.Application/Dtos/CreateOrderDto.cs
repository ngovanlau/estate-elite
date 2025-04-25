namespace PaymentService.Application.Dtos;

public class CreateOrderDto
{
    public bool Success { get; set; }
    public List<string> Links { get; set; } = [];
    public Guid TransactionId { get; set; }
    public required string OrderId { get; set; }
}
