using AutoMapper;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio.Exceptions;
using PaymentService.Application.Dtos;
using PaymentService.Application.Interfaces;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using SharedKernel.Enums;
using SharedKernel.Protos;
using SharedKernel.Settings;

namespace PaymentService.Infrastructure.Services;

public class PaypalService : IPaypalService
{
    private readonly PaypalSetting _setting;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly OrdersController _ordersController;

    public PaypalService(
        ILogger<PaypalService> logger,
        IMapper mapper,
        IOptions<PaypalSetting> options)
    {
        _setting = options.Value;
        _logger = logger;
        _mapper = mapper;

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
        decimal amount,
        string propertyTitle,
        CurrencyUnit currencyUnit,
        Guid sellerId,
        string returnUrl,
        string cancelUrl,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var seller = await GetUserByGrpcAsync(sellerId, cancellationToken) ?? throw new ObjectNotFoundException("Seller not found");

            var transactionId = Guid.NewGuid();

            var createOrderInput = new CreateOrderInput
            {
                Prefer = "return=representation",
                Body = CreateOrderRequest(amount, propertyTitle, currencyUnit, sellerId, seller, transactionId, returnUrl, cancelUrl)
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
       decimal amount,
       string propertyTitle,
       CurrencyUnit currencyUnit,
       Guid sellerId,
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
                    ReferenceId = sellerId.ToString(),
                    Description = $"Payment for {propertyTitle}",
                    CustomId = Guid.NewGuid().ToString(),
                    SoftDescriptor = "Estate Elite",
                    InvoiceId = transactionId.ToString(),
                    Payee = new PayeeBase
                    {
                        EmailAddress = seller.PayPalEmail,
                        MerchantId = seller.PayPalMerchantId
                    },
                    Amount = new AmountWithBreakdown
                    {
                        CurrencyCode = currencyUnit.ToString(),
                        MValue = amount.ToString("F2"), // Format to 2 decimal places
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

    private async Task<SellerDto> GetUserByGrpcAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:5101");
        var client = new UserService.UserServiceClient(channel);

        var request = new GetUserRequest
        {
            Id = userId.ToString()
        };

        try
        {
            var response = await client.GetUserAsync(request, cancellationToken: cancellationToken);
            if (response == null)
            {
                throw new Exception("User not found");
            }

            return _mapper.Map<SellerDto>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting seller details");
            throw;
        }
    }
}