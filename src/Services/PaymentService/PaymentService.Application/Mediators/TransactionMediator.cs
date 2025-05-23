using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Commands;
using PaymentService.Application.Requests;
using SharedKernel.Responses;

namespace PaymentService.Application.Mediators;

public static class TransactionMediator
{
    public static void AddTransactionMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<RentPropertyRequest, ApiResponse>, RentPropertyHandler>(life);
        configuration.AddBehavior<IRequestHandler<CaptureOrderRequest, ApiResponse>, CaptureOrderHandler>(life);
    }
}
