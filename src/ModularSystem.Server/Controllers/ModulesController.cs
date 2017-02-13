using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Repositories;
using ModularSystem.Communication.Data;
using ModularSystem.Server.Models;

namespace ModularSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ModulesController : Controller
    {
        private readonly IModulesRepository _modulesRepository;

        public ModulesController(IModulesRepository modulesRepository)
        {
            _modulesRepository = modulesRepository;
        }

        [HttpPost]
        [FaultContract(typeof(ArgumentException))]
        [Authorize(Roles = "Administrators")]
        public async Task InstallModuleAsync(ModuleDto module)
        {
            _modulesRepository.RegisterModule(await module.Unwrap());
        }

        [HttpPut]
        [Authorize(Roles = "Administrators")]
        public async Task RemoveModuleAsync(ModuleIdentity module)
        {
            _modulesRepository.UnregisterModule(module);
        }

        [HttpGet]
        public async Task<DownloadModulesResponse> DownloadModulesAsync(DownloadModulesRequest request)
        {
            List<ModuleDto> res = new List<ModuleDto>();
            foreach (var moduleIdentity in request.ModuleIdentities)
            {
                if (moduleIdentity.ModuleType != ModuleType.Client)
                    throw new WrongModuleTypeException(moduleIdentity, new[] { ModuleType.Client });
                var module = _modulesRepository.GetModule(moduleIdentity);
                if (module == null)
                    throw new ModuleMissedException(moduleIdentity);
                res.Add(await module.Wrap());
            }
            return new DownloadModulesResponse(res);
        }
    }
}
