using System;
using System.Collections.Generic;
using System.Linq;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Common.Dependencies
{
    public class DependenciesResolver
    {
        private readonly IModulesRepository<ClientModuleBase> _clientModules;
        private readonly IModulesRepository<ServerModuleBase> _serverModules;
        private readonly IUserModulesRepository _userModules;

        public DependenciesResolver(IModulesRepository<ClientModuleBase> clientModules, IModulesRepository<ServerModuleBase> serverModules, IUserModulesRepository userModules)
        {
            _clientModules = clientModules;
            _serverModules = serverModules;
            _userModules = userModules;
        }

        #region Helpers

        /// <summary>
        /// Check if there is no already installed module with same identity.
        /// </summary>
        private ICheckResult CheckDuplicates(IEnumerable<IModule> modules)
        {
            foreach (var module in modules)
            {
                switch (module.Type)
                {
                    case ModuleType.Client:
                        if (_clientModules.ContainsModule(module.ModuleIdentity))
                            return new ModuleDuplicatedError(module);
                        break;
                    case ModuleType.Server:
                        if (_serverModules.ContainsModule(module.ModuleIdentity))
                            return new ModuleDuplicatedError(module);
                        break;
                }
            }
            return new SuccessResult();
        }

        /// <summary>
        /// Check if all server module dependecies are in repositories (server).
        /// </summary>
        /// <param name="module">For this module check will be done</param>
        /// <param name="modulesGroup">Modules can be installed in groups, in this case missed module can be later in group (will be installed, but later) </param>
        private ICheckResult CheckDependeciesInRep(ServerModuleBase module, IModulesGroup modulesGroup = null)
        {
            foreach (var moduleServerDependency in module.ServerDependencies)
            {
                if (!_serverModules.ContainsModule(moduleServerDependency) && 
                    modulesGroup != null && 
                    !modulesGroup.ContainsServerModule(moduleServerDependency))
                    return new MissedModuleError(module, moduleServerDependency, ModuleType.Server);
            }
            return new SuccessResult();
        }

        /// <summary>
        /// Check if all client module dependecies are in repositories (server and client).
        /// </summary>
        /// <param name="module">For this module check will be done</param>
        /// <param name="modulesGroup">Modules can be installed in groups, in this case missed module can be later in group (will be installed, but later) </param>
        private ICheckResult CheckDependeciesInRep(ClientModuleBase module, IModulesGroup modulesGroup = null)
        {
            foreach (var moduleServerDependency in module.ServerDependencies)
            {
                if (!_serverModules.ContainsModule(moduleServerDependency) &&
                    modulesGroup != null &&
                    modulesGroup.ContainsServerModule(moduleServerDependency))
                    return new MissedModuleError(module, moduleServerDependency, ModuleType.Server);
            }
            foreach (var moduleClientDependency in module.ClientDependencies)
            {
                if (!_clientModules.ContainsModule(moduleClientDependency) &&
                    modulesGroup != null &&
                    modulesGroup.ContainsClientModule(moduleClientDependency))
                    return new MissedModuleError(module, moduleClientDependency, ModuleType.Server);
            }
            return new SuccessResult();
        }

        /// <summary>
        /// Check if client module can be resolved using specifed client modules scope (specifed userId and clientId)
        /// </summary>
        private ICheckResult CheckClientModulesScope(ClientModuleBase module, string userId, string clientId)
        {
            foreach (var moduleClientDependency in module.ClientDependencies)
            {
                if (!_userModules.Contains(userId, clientId, moduleClientDependency))
                {
                    return new MissedInClientScopeError(module, moduleClientDependency, userId, clientId);
                }
            }
            return new SuccessResult();
        }

        #endregion Helpers

        /// <summary>
        /// Check if list of modules can be installed to system.
        /// </summary>
        public virtual ICheckResult CheckCanBeInstalled(IModulesGroup modules, string userId = null, string clientId = null)
        {
            List<ICheckResult> errors = new List<ICheckResult>();
            foreach (var serverModule in modules.ServerModules)
            {
                // Duplicate check
                if (_serverModules.ContainsModule(serverModule.ModuleIdentity))
                {
                    errors.Add(new ModuleDuplicatedError(serverModule));
                }
                // Dependecies in repository check
                var res = CheckDependeciesInRep(serverModule, modules);
                if (!res.IsSuccess)
                    errors.Add(res);
            }
            foreach (var clientModule in modules.ClientModules)
            {
                // Duplicate check
                if (_clientModules.ContainsModule(clientModule.ModuleIdentity))
                {
                    errors.Add(new ModuleDuplicatedError(clientModule));
                }
                // Dependecies in repository check
                {
                    var res = CheckDependeciesInRep(clientModule, modules);
                    if (!res.IsSuccess)
                        errors.Add(res);
                }
                // Check dependecies in client scope
                if (userId != null && clientId != null)
                {
                    var res = CheckClientModulesScope(clientModule, userId, clientId);
                    if (!res.IsSuccess)
                        errors.Add(res);
                }
            }
            if (errors.Count == 0)
                return new SuccessResult();
            return new ComplexError(errors.ToArray());
        }
    }
}
