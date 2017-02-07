using System;
using System.Threading.Tasks;
using ModularSystem.Communication.Contracts;
using ModularSystem.Communication.Data;

namespace ModularSystem.Server.Services
{
    public class ModulesService : IModulesService
    {
        /// <inheritdoc />
        public Task<IResolveResponse> ResolveAsync(IResolveRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
