﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Modules;
using ModularSystem.Communication.Data.Files;
using ModularSystem.Server.Common;

namespace ModularSystem.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class ModulesController : Controller
    {
        private readonly RegisteredModules _registeredModules;

        public ModulesController(RegisteredModules registeredModules)
        {
            _registeredModules = registeredModules;
        }

        #region Modules
        [HttpPost("install")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleMissedException), HttpStatusCode.BadRequest)]
        [MappedExceptionFilter(typeof(ArgumentException), HttpStatusCode.BadRequest)] // May be not safe
        public async Task InstallModulePackageAsync()
        {
            var package = await ModulesPackage.Decompress(Request.Body);
            _registeredModules.RegisterModules(package.PackagedModules);
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
            var dtos = modules.Select(x => x.ModuleInfo.ModuleIdentity.ToString()).ToArray();
            return dtos;
        }
        #endregion

        #region User modules
        [HttpPost("user/{userId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        public void AddUserModules(string userId, [FromBody]IEnumerable<string> moduleIdentities)
        {
            _registeredModules.AddModules(userId, moduleIdentities.Select(ModuleIdentity.Parse));
        }

        [HttpDelete("user/{userId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        [MappedExceptionFilter(typeof(ModuleIsRequiredException), HttpStatusCode.BadRequest)]
        public void RemoveUserModules(string userId, IEnumerable<string> moduleIdentities)
        {
            _registeredModules.RemoveModules(userId, moduleIdentities.Select(ModuleIdentity.Parse));
        }

        [HttpGet("user/{userId}")]
        // TODO: allow user gets its modules
        [Authorize(Policy = "ConfigModulesAllowed")]
        public IEnumerable<string> GetUserModules(string userId)
        {
            var r = _registeredModules.GetModuleIdentities(userId);
            if (r == null)
                return new string[0];
            return _registeredModules.GetModuleIdentities(userId).Select(x => x.ToString());
        }

        [HttpGet("download")]
        [Authorize]
        public async Task<IActionResult> DownloadModulesAsync()
        {
            var userId = User.FindFirst("sub");
            if (userId == null)
                return Forbid();

            var package = new ModulesPackage(_registeredModules.GetModules(userId.Value));
            return File(await package.Compress(), "application/zip");
        }
        #endregion
    }
}
