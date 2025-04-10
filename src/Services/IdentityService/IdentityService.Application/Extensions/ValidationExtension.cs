using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Extensions;

using System.Reflection;

public static class ValidationExtension
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
        => services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}