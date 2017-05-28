using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.PackedModules.Zip;
using ModularSystem.Server.Common;

namespace ModularSystem.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class ModulesController : Controller
    {
        private readonly ClientModulesManager _clientModules;
        private readonly ServerModulesManager _serverModules;
        private readonly UserModulesManager _userModules;

        private ModulesManager _modulesManager;

        public ModulesController(ClientModulesManager clientModules, ServerModulesManager serverModules, UserModulesManager userModules)
        {
            _clientModules = clientModules;
            _serverModules = serverModules;
            _userModules = userModules;

            _modulesManager = new ModulesManager(_clientModules, _serverModules, _userModules);
        }

        [HttpGet("download")]
        [Authorize]
        public IActionResult ResolveModulesAsync()
        {
            var userId = User.FindFirst("sub");
            if (userId == null)
                return Forbid();

            var clientType = User.FindFirst("client_id");
            if (clientType == null)
                return Forbid();

            var batch = _modulesManager.ResolveClientModules(userId.Value, clientType.Value);
            return File(batch.ExtractData(), "application/zip");
        }
    }
}
