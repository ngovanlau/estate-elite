using Common.Application.Responses;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Commands;
using PaymentService.Application.Requests;

namespace PaymentService.Application.Mediators;

public static class TransactionMediator
{
    public static void AddTransactionMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<RentPropertyRequest, ApiResponse>, RentPropertyHandler>(life);
        configuration.AddBehavior<IRequestHandler<CaptureOrderRequest, ApiResponse>, CaptureOrderHandler>(life);
    }
}
