namespace Microsoft.Extensions.DependencyInjection.Discovery
{
    public interface IServiceDiscoveryBootstrapper
    {
        void ConfigureServices(IServiceCollection services);
    }
}
