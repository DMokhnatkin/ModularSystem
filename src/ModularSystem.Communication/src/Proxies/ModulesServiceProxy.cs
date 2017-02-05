using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using ModularSystem.Communication.Contracts;
using ModularSystem.Communication.Data;

namespace ModularSystem.Communication.Proxies
{
    public class ModulesServiceProxy : ClientBase<IModulesService>, IModulesService
    {
        public ModulesServiceProxy()
        { }

        public ModulesServiceProxy(Binding binding, EndpointAddress address) : base(binding, address)
        { }

        /// <inheritdoc />
        public Task<ResolveResponse> ResolveAsync(ResolveRequest req)
        {
            return Channel.ResolveAsync(req);
        }
    }
}
