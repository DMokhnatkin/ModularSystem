using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.PackedModules.Zip;
using ModularSystem.Server.Common;

namespace ModularSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ConfigModulesAllowed")]
    public class ModulesConfigController : Controller
    {
        private readonly ModulesManager _modulesManager;

        public ModulesConfigController(ModulesManager modulesManager)
        {
            _modulesManager = modulesManager;
        }

        [HttpPost("install")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleMissedException), HttpStatusCode.BadRequest)]
        [MappedExceptionFilter(typeof(ArgumentException), HttpStatusCode.BadRequest)] // May be not safe
        public void InstallModulesBatchAsync()
        {
            using (var t = Request.Body)
            using (var ms = new MemoryStream())
            {
                t.CopyTo(ms);
                var batchedModules = new MemoryBatchedModules(ms.ToArray());
                _modulesManager.InstallBatch(batchedModules);
            }
        }

        [HttpPut("remove")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        [MappedExceptionFilter(typeof(ArgumentException), HttpStatusCode.BadRequest)] // May be not safe
        public async Task RemoveModuleAsync([FromBody]string module)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public string[] GetModulesListAsync()
        {
            return _modulesManager.GetInstalledModules().Select(x => x.ToString()).ToArray();
        }

        [HttpPost("user/{userId}/{clientId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        public IActionResult AddUserModules(string userId, string clientId, [FromBody]IEnumerable<string> moduleIdentities)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("user/{userId}/{clientId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        public IActionResult RemoveUserModules(string userId, string clientId, IEnumerable<string> moduleIdentities)
        {
            throw new NotImplementedException();
        }

        [HttpGet("user/{userId}")]
        // TODO: allow user gets its modules
        [Authorize(Policy = "ConfigModulesAllowed")]
        public IEnumerable<string> GetUserModules(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
