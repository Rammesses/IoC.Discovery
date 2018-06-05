using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection.Discovery
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection BootstrapFrom<T>(this IServiceCollection services, T bootstrapper)
            where T : IServiceDiscoveryBootstrapper
        {
            bootstrapper.ConfigureServices(services);
            return services;
        }

        public static IServiceCollection BootstrapFrom<T>(this IServiceCollection services)
            where T : IServiceDiscoveryBootstrapper
        {
            if (!services.Any(s => s.ServiceType == typeof(T)))
            {
                services.AddSingleton(typeof(T));
            }

            var serviceProviderFactory = new DefaultServiceProviderFactory();
            var serviceProvider = serviceProviderFactory.CreateServiceProvider(services);

            var instance = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<T>();

            services.BootstrapFrom(instance);

            return services;
        }

        public static IServiceCollection BootstrapByDiscovery(this IServiceCollection services)
        {
            services.BootstrapFrom<ConventionBasedDiscoveryBootstrapper>();
            return services;
        }
    }
}
