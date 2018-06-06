using System.Composition.Hosting;

namespace Microsoft.Extensions.DependencyInjection.Discovery
{
    public static class ContainerConfigurationExtensions
    {
        public static ContainerConfiguration WithExistingServices(this ContainerConfiguration config, IServiceCollection services)
        {
            config.WithProvider(new ServiceExportProvider(services));
            return config;
        }
    }
}
