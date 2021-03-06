﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ConfigModulesAllowed")]
    public class ModulesConfigController : Controller
    {
        private readonly ServerModulesManager _serverModulesManager;
        private readonly ClientModulesManager _clientModulesManager;
        private readonly UserModulesManager _userModulesManager;

        private ModulesManager _modulesManager;

        public ModulesConfigController(ServerModulesManager serverModulesManager, ClientModulesManager clientModulesManager, UserModulesManager userModulesManager)
        {
            _serverModulesManager = serverModulesManager;
            _clientModulesManager = clientModulesManager;
            _userModulesManager = userModulesManager;

            _modulesManager = new ModulesManager(_clientModulesManager, _serverModulesManager, _userModulesManager);
        }

        [HttpPost("install")]
        [Authorize(Policy = "ConfigModulesAllowed")]
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
        public async Task RemoveModuleAsync([FromBody]string module)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public string[] GetModulesListAsync()
        {
            return _modulesManager.GetInstalledList().Select(x => x.ToString()).ToArray();
        }

        [HttpPost("user/{userId}/{clientId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
        public IActionResult RegisterUserModules(string userId, string clientId, [FromBody]IEnumerable<string> moduleIdentities)
        {
            _userModulesManager.RegisterModulesForUser(userId, clientId, moduleIdentities.Select(ModuleIdentity.Parse));
            return Ok();
        }

        [HttpDelete("user/{userId}/{clientId}")]
        [Authorize(Policy = "ConfigModulesAllowed")]
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
