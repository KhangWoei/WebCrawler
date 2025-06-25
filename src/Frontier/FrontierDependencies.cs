using Microsoft.Extensions.DependencyInjection;

namespace Frontier;

public static class FrontierDependencies
{
    public static void UseFrontier(this IServiceCollection services)
    {
        services.AddSingleton<Controller>();
    }
}