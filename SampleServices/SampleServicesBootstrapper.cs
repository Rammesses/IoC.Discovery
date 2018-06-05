using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Discovery;

namespace SampleServices
{
    public sealed class SampleServicesBootstrapper : IServiceDiscoveryBootstrapper
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MyCustomService>();
        }
    }
}
