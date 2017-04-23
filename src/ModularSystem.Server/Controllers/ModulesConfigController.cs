using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Dependencies;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.PackedModules.Zip;
using ModularSystem.Server.Common;

namespace ModularSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ConfigModulesAllowed")]
    public class ModulesConfigController : Controller
    {
        private readonly RegisteredModules _registeredModules;
        private readonly DependenciesResolver _dependenciesResolver;

        public ModulesConfigController(RegisteredModules registeredModules, DependenciesResolver dependenciesResolver)
        {
            _registeredModules = registeredModules;
            _dependenciesResolver = dependenciesResolver;
        }

        [HttpPost("install")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleMissedException), HttpStatusCode.BadRequest)]
        [MappedExceptionFilter(typeof(ArgumentException), HttpStatusCode.BadRequest)] // May be not safe
        public async Task InstallModulePackageAsync()
        {
            using (var t = Request.Body)
            using (var br = new BinaryReader(t))
            {
                var batchedModules = new MemoryBatchedModules(br.ReadBytes((int)t.Length));
                MemoryPackedModule[] innerModules;
                batchedModules.UnbatchModules(out innerModules);
                _registeredModules.RegisterModules(innerModules);
            }
        }

        [HttpPost("install")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleMissedException), HttpStatusCode.BadRequest)]
        [MappedExceptionFilter(typeof(ArgumentException), HttpStatusCode.BadRequest)] // May be not safe
        public async Task InstallModulePackageV2Async()
        {
            using (var t = Request.Body)
            using (var br = new BinaryReader(t))
            {
                var batchedModules = new MemoryBatchedModules(br.ReadBytes((int)t.Length));
                MemoryPackedModule[] innerModules;
                batchedModules.UnbatchModules(out innerModules);

                

                _registeredModules.RegisterModules(innerModules);
            }
        }

        [HttpPut("remove")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        [MappedExceptionFilter(typeof(ArgumentException), HttpStatusCode.BadRequest)] // May be not safe
        public async Task RemoveModuleAsync([FromBody]string module)
        {
            await Task.Factory.StartNew(() => _registeredModules.UnregisterModule(ModuleIdentity.Parse(module)));
        }

        [HttpGet]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public async Task<string[]> GetModulesListAsync()
        {
            var modules = await Task.Factory.StartNew(() => _registeredModules.GetRegisteredModules());
            var dtos = modules.Select(x => x.ModuleIdentity.ToString()).ToArray();
            return dtos;
        }

        [HttpPost("user/{userId}/{clientId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        public IActionResult AddUserModules(string userId, string clientId, [FromBody]IEnumerable<string> moduleIdentities)
        {
            _registeredModules.AddModules(userId, clientId, moduleIdentities.Select(ModuleIdentity.Parse));
            return Ok();
        }

        [HttpDelete("user/{userId}/{clientId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        public IActionResult RemoveUserModules(string userId, string clientId, IEnumerable<string> moduleIdentities)
        {
            _registeredModules.RemoveModules(userId, clientId, moduleIdentities.Select(ModuleIdentity.Parse));
            return Ok();
        }

        [HttpGet("user/{userId}")]
        // TODO: allow user gets its modules
        [Authorize(Policy = "ConfigModulesAllowed")]
        public IEnumerable<string> GetUserModules(string userId)
        {
            var r = _registeredModules.GetModuleIdentities(userId, "wpfclient");
            if (r == null)
                return new string[0];
            return _registeredModules.GetModuleIdentities(userId, "wpfclient").Select(x => x.ToString());
        }
    }
}
