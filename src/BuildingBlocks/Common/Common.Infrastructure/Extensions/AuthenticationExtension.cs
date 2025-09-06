using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Common.Domain.Constants;
using Common.Infrastructure.Implements;
using Common.Application.Interfaces;
using Common.Infrastructure.Settings;

namespace Common.Infrastructure.Extensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSetting>(configuration.GetSection("Jwt"));

        var jwtSetting = configuration.GetSection("Jwt").Get<JwtSetting>();
        if (jwtSetting is null)
        {
            throw new ArgumentNullException(nameof(jwtSetting), "Jwt settings are not configured.");
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSetting.Issuer,
                ValidAudience = jwtSetting.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
                ClockSkew = TimeSpan.Zero // Remove delay of token when expire
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireBuyerRole", policy => policy.RequireRole(RoleName.Buyer));
            options.AddPolicy("RequireSellerRole", policy => policy.RequireRole(RoleName.Seller));
            options.AddPolicy("RequireAminRole", policy => policy.RequireRole(RoleName.Admin));
        });

        // Defines a contract for accessing current authenticated user's claims
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
