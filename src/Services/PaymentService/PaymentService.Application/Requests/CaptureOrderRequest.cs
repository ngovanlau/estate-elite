using Common.Application.Responses;
using MediatR;

namespace PaymentService.Application.Requests;

public class CaptureOrderRequest : IRequest<ApiResponse>
{
    public Guid TransactionId { get; set; }
    public required string OrderId { get; set; }
}
