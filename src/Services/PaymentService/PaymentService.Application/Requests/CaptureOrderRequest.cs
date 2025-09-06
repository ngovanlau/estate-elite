using MediatR;
using Common.Application.Responses;

namespace PaymentService.Application.Requests;

public class CaptureOrderRequest : IRequest<ApiResponse>
{
    public Guid TransactionId { get; set; }
    public required string OrderId { get; set; }
}
