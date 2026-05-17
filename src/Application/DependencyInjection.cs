using System.Reflection;
using Porcupine.Application.Common.Behaviors;
using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(config => 
            config.AddMaps(Assembly.GetExecutingAssembly())
        );

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        });

        var triggerService = new TriggerService();
        triggerService.AddTriggersFromAppDomain();
        services.AddSingleton<ITriggerService>(triggerService);

        return services;
    }
}
