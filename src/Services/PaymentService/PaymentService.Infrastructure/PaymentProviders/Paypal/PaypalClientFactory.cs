using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using SharedKernel.Settings;

namespace PaymentService.Infrastructure.PaymentProviders.Paypal;

public class PaypalClientFactory(IOptions<PaypalSetting> options)
{
    private readonly PaypalSetting _setting = options.Value;

    public PaypalServerSdkClient CreateClient()
    {
        return new PaypalServerSdkClient.Builder()
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
    }
}
