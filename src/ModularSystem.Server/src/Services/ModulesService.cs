using System;
using System.Threading.Tasks;
using ModularSystem.Common.Repositories;
using ModularSystem.Communication.Contracts;
using ModularSystem.Communication.Data;

namespace ModularSystem.Server.Services
{
    public class ModulesService : IModulesService
    {
        private readonly IModulesRepository _modulesRepository;

        public ModulesService(IModulesRepository modulesRepository)
        {
            _modulesRepository = modulesRepository;
        }

        /// <inheritdoc />
        public Task<IResolveResponse> ResolveAsync(IResolveRequest req)
        {
           throw new NotImplementedException();
        }
    }
}
