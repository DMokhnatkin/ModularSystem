using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using ModularSystem.Common;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Repositories;
using ModularSystem.Communication.Contracts;
using ModularSystem.Communication.Data;
using ModularSystem.Server.Data;

namespace ModularSystem.Server.Services
{
    public class ModulesService : IModuleFilesService
    {
        public ModulesService(IModulesRepository modulesRepository)
        {
            ModulesRepository = modulesRepository;
        }

        public IModulesRepository ModulesRepository { get; }

        /// <inheritdoc />
        [FaultContract(typeof(WrongModuleTypeException))]
        [FaultContract(typeof(ModuleMissedException))]
        [FaultContract(typeof(ModuleIsRequiredException))]
        public async Task<IDownloadModulesResponse> DownloadModules(IDownloadModulesRequest request)
        {
            List<ModuleDto> res = new List<ModuleDto>();
            foreach (var moduleIdentity in request.ModuleIdentities)
            {
                if (moduleIdentity.ModuleType != ModuleType.Client)
                    throw new WrongModuleTypeException(moduleIdentity, new []{ ModuleType.Client });
                var module = ModulesRepository.GetModule(moduleIdentity);
                if (module == null)
                    throw new ModuleMissedException(moduleIdentity);
                res.Add(await module.Wrap());
            }
            return new DownloadModulesResponse(res);
        }
    }
}
