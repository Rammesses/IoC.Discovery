using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.Discovery
{
    internal class ConventionBasedDiscoveryBootstrapper : IServiceDiscoveryBootstrapper
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var conventions = new ConventionBuilder();
            conventions
                .ForTypesDerivedFrom<IServiceDiscoveryBootstrapper>()
                .Export<IServiceDiscoveryBootstrapper>()
                .Shared();

            var baseAssembly = Assembly.GetEntryAssembly();
            var referencedAssemblies = baseAssembly.GetReferencedAssemblies().Select(n => Assembly.Load(n));
            var configuration = new ContainerConfiguration()
                .WithAssembly(baseAssembly, conventions)
                .WithAssemblies(referencedAssemblies, conventions)
                .WithExistingServices(services);

            IEnumerable<IServiceDiscoveryBootstrapper> discoveredBootstrappers = null;
            using (var container = configuration.CreateContainer())
            {
                discoveredBootstrappers = container.GetExports<IServiceDiscoveryBootstrapper>()
                    .Where(b => b.GetType() != typeof(ConventionBasedDiscoveryBootstrapper));
            }

            if (!discoveredBootstrappers.Any())
            {
                return;
            }

            foreach(var bootstrapper in discoveredBootstrappers)
            {
                services.BootstrapFrom(bootstrapper);
            }
        }
    }
}