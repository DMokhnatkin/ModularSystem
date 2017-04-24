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

        /// <summary>
        /// Check if modules can be resolved after add to user. (check all module dependencies)
        /// </summary>
        public virtual ICheckResult CheckAddForUser(string userId, string clientId, IEnumerable<IModule> modules)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if list of modules can be installed to system.
        /// </summary>
        public virtual ICheckResult CheckCanBeInstalled(IEnumerable<IModule> modules)
        {
            throw new NotImplementedException();
        }
    }
}
