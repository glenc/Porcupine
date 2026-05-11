using Azure.Identity;
using Porcupine.Application.Common.Interfaces;
using Porcupine.Infrastructure.Data;
using Porcupine.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Porcupine.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
}
