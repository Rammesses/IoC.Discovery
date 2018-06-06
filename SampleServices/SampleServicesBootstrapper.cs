using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Discovery;

namespace SampleServices
{
    public sealed class SampleServicesBootstrapper : IServiceDiscoveryBootstrapper
    {
        private readonly IHttpContextAccessor contextAccessor;

        public SampleServicesBootstrapper(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MyCustomService>(new MyCustomService(contextAccessor));
        }
    }
}
