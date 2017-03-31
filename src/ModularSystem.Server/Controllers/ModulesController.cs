﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
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

        [HttpGet("download")]
        [Authorize]
        [MappedExceptionFilter(typeof(ModuleMissedException), HttpStatusCode.Conflict)]
        public async Task<IActionResult> DownloadModulesAsync()
        {
            var userId = User.FindFirst("sub");
            if (userId == null)
                return Forbid();

            var clientId = User.FindFirst("client_id");
            if (clientId == null)
                return Forbid();

            var package = new ModulesPackage(_registeredModules.GetModules(userId.Value, clientId.Value));
            return File(await package.Compress(), "application/zip");
        }
    }
}
