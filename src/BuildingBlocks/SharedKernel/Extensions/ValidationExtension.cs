using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedKernel.Extensions;

public static class ValidationExtension
{
    public static IServiceCollection AddValidation(this IServiceCollection services, Assembly assembly)
        => services.AddValidatorsFromAssembly(assembly);
}