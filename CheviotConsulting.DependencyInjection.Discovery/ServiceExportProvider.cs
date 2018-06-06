using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection.Discovery
{
    internal class ServiceExportProvider : ExportDescriptorProvider
    {
        private IServiceCollection services;

        public ServiceExportProvider(IServiceCollection services)
        {
            this.services = services;
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(
            CompositionContract contract, 
            DependencyAccessor descriptorAccessor)
        {
            var result = new List<ExportDescriptorPromise>();
            foreach (var service in services.Where(s => s.ServiceType == contract.ContractType))
            {
                var servicePromise = new ExportDescriptorPromise(
                    contract, 
                    "Services",
                    true,
                    NoDependencies,
                    _ => ExportDescriptor.Create((c, o) => services.QuickResolve(contract.ContractType), NoMetadata));
                result.Add(servicePromise);
            }

            return result;
        }
    }
}