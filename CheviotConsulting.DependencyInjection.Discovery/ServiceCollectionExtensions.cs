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

        internal static object QuickResolve(this IServiceCollection services, Type targetType)
        {
            var serviceProviderFactory = new DefaultServiceProviderFactory();
            var serviceProvider = serviceProviderFactory.CreateServiceProvider(services);

            var instance = serviceProvider.CreateScope().ServiceProvider.GetRequiredService(targetType);
            return instance;
        }

        internal static T QuickResolve<T>(this IServiceCollection services)
        {
            return (T)services.QuickResolve(typeof(T));
        }

        public static IServiceCollection BootstrapFrom<T>(this IServiceCollection services)
            where T : IServiceDiscoveryBootstrapper
        {
            if (!services.Any(s => s.ServiceType == typeof(T)))
            {
                services.AddSingleton(typeof(T));
            }

            var instance = services.QuickResolve<T>();
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
