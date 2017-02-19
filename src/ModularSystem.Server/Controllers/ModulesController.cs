using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Communication.Data;
using ModularSystem.Communication.Data.Dto;
using ModularSystem.Communication.Data.Mappers;
using ModularSystem.Server.Models;

namespace ModularSystem.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class ModulesController : Controller
    {
        private readonly Modules _modules;
        private readonly UserModules _userModules;

        public ModulesController(Modules modules, UserModules userModules)
        {
            _modules = modules;
            _userModules = userModules;
        }

        [HttpPost("install")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [FaultContract(typeof(ArgumentException))]
        public async Task InstallModuleAsync([FromBody]ModuleDto module)
        {
            _modules.RegisterModule(await module.Unwrap());
        }

        [HttpPut("remove")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public async Task RemoveModuleAsync(ModuleIdentity module)
        {
            _modules.UnregisterModule(module);
        }

        [HttpGet]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public async Task<ModuleIdentityDto[]> GetModulesListAsync()
        {
            var modules = await Task.Factory.StartNew(() => _modules.GetRegisteredModules());
            var dtos = modules.Select(x => x.ModuleInfo.ModuleIdentity.Wrap()).ToArray();
            return dtos;
        }

        [HttpGet("download")]
        public async Task<DownloadModulesResponse> DownloadModulesAsync(DownloadModulesRequest request)
        {
            List<ModuleDto> res = new List<ModuleDto>();
            foreach (var moduleIdentity in request.ModuleIdentities)
            {
                if (moduleIdentity.ModuleType != ModuleType.Client)
                    throw new WrongModuleTypeException(moduleIdentity, new[] { ModuleType.Client });
                var module = _modules.GetModule(moduleIdentity);
                if (module == null)
                    throw new ModuleMissedException(moduleIdentity);
                res.Add(await module.Wrap());
            }
            return new DownloadModulesResponse(res);
        }

        [HttpGet("test")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public string Test()
        {
            return string.Concat(User.Claims.Select(x => x.Type.ToString() + " " + x.Value.ToString() + " "));
        }
    }
}
