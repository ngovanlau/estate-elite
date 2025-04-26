using PaymentService.Application.Dtos;
using SharedKernel.Enums;

namespace PaymentService.Application.Interfaces;

public interface IPaypalService
{
    Task<CreateOrderDto> CreateOrderAsync(double amount, string propertyTitle, CurrencyUnit currencyUnit, Guid sellerId, string returnUrl, string cancelUrl, CancellationToken cancellationToken = default);
    Task<bool> CaptureOrderAsync(string orderId, CancellationToken cancellationToken = default);
    // Task<PaymentDetails> GetPaymentDetailsAsync(string orderId);
}
