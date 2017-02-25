using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Communication;
using ModularSystem.Communication.Data;
using ModularSystem.Communication.Data.Dto;
using ModularSystem.Communication.Data.Files;
using ModularSystem.Communication.Data.Mappers;
using ModularSystem.Server.Common;
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

        /*
        [HttpPost("install")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        //[FaultContract(typeof(ArgumentException))]
        public async Task InstallModuleAsync([FromBody]ModuleDto module)
        {
            _modules.RegisterModule(await module.Unwrap());
        }*/

        [HttpPost("install")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleMissedException), HttpStatusCode.BadRequest)]
        [MappedExceptionFilter(typeof(ArgumentException), HttpStatusCode.BadRequest)] // May be not safe
        public async Task InstallModulePackageAsync()
        {
            // TODO: use model bind
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            using (ZipArchive modulePackage = new ZipArchive(Request.Body))
            {
                modulePackage.ExtractToDirectory(tempPath);
            }
            var modules = new List<IModule>();
            foreach (var t in Directory.GetDirectories(tempPath))
            {
                modules.Add(await ModuleDtoFileSystem.ReadFromDirectory(t).Unwrap());
            }
            _modules.RegisterModules(modules);
        }

        [HttpPut("remove")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        [MappedExceptionFilter(typeof(ArgumentException), HttpStatusCode.BadRequest)] // May be not safe
        public async Task RemoveModuleAsync([FromBody]ModuleIdentityDto module)
        {
            await Task.Factory.StartNew(() => _modules.UnregisterModule(module.Unwrap()));
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

        [HttpPost("addForUser/{userId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        public void AddUserModules(string userId, [FromBody]IEnumerable<ModuleIdentityDto> moduleIdentity)
        {
            _userModules.AddModules(userId, moduleIdentity.Select(x => x.Unwrap()));
        }

        [HttpGet("getForUser/{userId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public IEnumerable<ModuleIdentityDto> GetUserModules(string userId)
        {
            var r = _userModules.GetModules(userId);
            if (r == null)
                return new ModuleIdentityDto[0];
            return _userModules.GetModules(userId).Select(x => x.Wrap());
        }

        [HttpGet("test")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public string Test()
        {
            return string.Concat(User.Claims.Select(x => x.Type.ToString() + " " + x.Value.ToString() + " "));
        }
    }
}
