using PaymentService.Application.Dtos;
using Common.Domain.Enums;

namespace PaymentService.Application.Interfaces;

public interface IPaypalService
{
    Task<CreateOrderDto> CreateOrderAsync(string propertyTile, double amount, CurrencyUnit currencyUnit, SellerDto seller, string returnUrl, string cancelUrl, CancellationToken cancellationToken = default);
    Task<bool> CaptureOrderAsync(string orderId, CancellationToken cancellationToken = default);
    // Task<PaymentDetails> GetPaymentDetailsAsync(string orderId);
}
