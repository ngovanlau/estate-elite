using System;
using MediatR;
using SharedKernel.Responses;

namespace PaymentService.Application.Requests;

public sealed record RentPropertyRequest : IRequest<ApiResponse>
{
    public Guid PropertyId { get; set; }
    public int RentalPeriod { get; set; }
    public required string ReturnUrl { get; set; }
    public required string CancelUrl { get; set; }
}
