using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Common.Infrastructure.Extensions;

public static class DataProtectionExtension
{
    public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration configuration)
    {
        var dataProtectionBuilder = services.AddDataProtection()
            .SetApplicationName(configuration["ServiceName"] + "")
            .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));

        if (configuration.GetValue<bool>("DataProtection:UseX509Certificate"))
        {
            var certThumbprint = configuration["DataProtection:CertificateThumbprint"];
            if (!string.IsNullOrEmpty(certThumbprint))
            {
                var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                var cert = store.Certificates
                    .Find(X509FindType.FindByThumbprint, certThumbprint, validOnly: false)
                    .OfType<X509Certificate2>()
                    .FirstOrDefault();

                if (cert != null)
                {
                    dataProtectionBuilder.ProtectKeysWithCertificate(cert);
                    Log.Information("DataProtection keys protected with X509 certificate.");
                }
                else
                {
                    Log.Warning("X509 certificate with thumbprint {Thumbprint} not found. Using default protection.", certThumbprint);
                }

                store.Close();
            }
        }

        return services;
    }
}
