using Microsoft.Extensions.Logging;
using PaymentService.Application.Dtos;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.PaymentProviders.Paypal;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using SharedKernel.Enums;

namespace PaymentService.Infrastructure.Services;

public class PaypalService(PaypalClientFactory factory, ILogger<PaypalService> logger) : IPaypalService
{
    private readonly OrdersController _ordersController = factory.CreateClient().OrdersController;

    public async Task<bool> CaptureOrderAsync(string orderId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(orderId))
        {
            throw new ArgumentException("Order ID cannot be null or empty", nameof(orderId));
        }

        try
        {
            var captureOrderInput = new CaptureOrderInput
            {
                Id = orderId
            };

            var response = await _ordersController.CaptureOrderAsync(captureOrderInput, cancellationToken);
            if (response.StatusCode != 201)
            {
                logger.LogWarning("Failed to capture PayPal order {OrderId}. Status code: {StatusCode}",
                    orderId, response.StatusCode);
                return false;
            }

            return response.Data.Status == OrderStatus.Completed;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error capturing PayPal order {OrderId}", orderId);
            throw;
        }
    }

    public async Task<CreateOrderDto> CreateOrderAsync(
        string propertyTile,
        double amount,
        CurrencyUnit currencyUnit,
        SellerDto seller,
        string returnUrl,
        string cancelUrl,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var transactionId = Guid.NewGuid();

            var createOrderInput = new CreateOrderInput
            {
                Body = CreateOrderRequest(propertyTile, amount, currencyUnit, seller, transactionId, returnUrl, cancelUrl)
            };

            var response = await _ordersController.CreateOrderAsync(createOrderInput, cancellationToken);

            if (response.StatusCode != 201 || response.Data.Status != OrderStatus.Created)
            {
                logger.LogError("Failed to create PayPal order. Status: {Status}, StatusCode: {StatusCode}", response.Data?.Status, response.StatusCode);
                throw new Exception("Failed to create PayPal order");
            }

            return new CreateOrderDto
            {
                Success = true,
                Links = response.Data.Links.Select(p => p.Href).ToList(),
                TransactionId = transactionId,
                OrderId = response.Data.Id
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create PayPal order");
            throw;
        }
    }

    private OrderRequest CreateOrderRequest(
        string propertyTile,
        double amount,
        CurrencyUnit currencyUnit,
        SellerDto seller,
        Guid transactionId,
        string returnUrl,
        string cancelUrl)
    {
        return new OrderRequest
        {
            Intent = CheckoutPaymentIntent.Capture,
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    ReferenceId = seller.Id.ToString(),
                    Description = $"Payment for {propertyTile}",
                    CustomId = Guid.NewGuid().ToString(),
                    SoftDescriptor = "Estate Elite",
                    InvoiceId = transactionId.ToString(),
                    Payee = new PayeeBase
                    {
                        EmailAddress = seller.PaypalEmail,
                        MerchantId = seller.PaypalMerchantId
                    },
                    Amount = new AmountWithBreakdown
                    {
                        CurrencyCode = currencyUnit.ToString(),
                        MValue = amount.ToString(),
                    }
                }
            },
            ApplicationContext = new OrderApplicationContext
            {
                ReturnUrl = returnUrl,
                CancelUrl = cancelUrl
            }
        };
    }
}