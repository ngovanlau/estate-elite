using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentService.Application.Dtos;
using PaymentService.Application.Interfaces;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using SharedKernel.Enums;
using SharedKernel.Settings;

namespace PaymentService.Infrastructure.Services;

public class PaypalService : IPaypalService
{
    private readonly PaypalSetting _setting;
    private readonly ILogger _logger;
    private readonly OrdersController _ordersController;

    public PaypalService(
        ILogger<PaypalService> logger,
        IOptions<PaypalSetting> options)
    {
        _setting = options.Value;
        _logger = logger;

        var client = new PaypalServerSdkClient.Builder()
            .ClientCredentialsAuth(
                new ClientCredentialsAuthModel.Builder(
                    _setting.ClientId,
                    _setting.ClientSecret
                )
                .Build())
            .Environment(_setting.UseSandbox
                ? PaypalServerSdk.Standard.Environment.Sandbox
                : PaypalServerSdk.Standard.Environment.Production)
            .LoggingConfig(config => config
                .LogLevel(LogLevel.Information)
                .RequestConfig(reqConfig => reqConfig.Body(true))
                .ResponseConfig(respConfig => respConfig.Headers(true))
            ).Build();

        _ordersController = client.OrdersController;
    }

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
            if (response.StatusCode != 200)
            {
                _logger.LogWarning("Failed to capture PayPal order {OrderId}. Status code: {StatusCode}",
                    orderId, response.StatusCode);
                return false;
            }

            return response.Data.Status == OrderStatus.Completed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error capturing PayPal order {OrderId}", orderId);
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
                Prefer = "return=representation",
                Body = CreateOrderRequest(propertyTile, amount, currencyUnit, seller, transactionId, returnUrl, cancelUrl)
            };

            var response = await _ordersController.CreateOrderAsync(createOrderInput, cancellationToken);

            if (response.StatusCode != 200 || response.Data.Status != OrderStatus.Created)
            {
                _logger.LogError("Failed to create PayPal order. Status: {Status}, StatusCode: {StatusCode}", response.Data?.Status, response.StatusCode);
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
            _logger.LogError(ex, "Failed to create PayPal order");
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