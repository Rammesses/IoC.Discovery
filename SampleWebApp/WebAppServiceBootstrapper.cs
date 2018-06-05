using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Discovery;

namespace SampleWebApp
{
    public sealed class WebAppServiceBootstrapper : IServiceDiscoveryBootstrapper
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IWebAppService, MyWebAppService>();
        }
    }
}
