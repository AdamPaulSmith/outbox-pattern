using Microsoft.Extensions.DependencyInjection;

namespace OutboxPattern.Customers.Core
{
    public static class CoreBootstrap
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }
    }
}