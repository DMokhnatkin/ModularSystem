using System;
using System.Collections.Generic;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Common.Dependencies
{
    public class DependenciesResolver
    {
        private readonly IModulesRepository<ClientModuleBase> _clientModules;
        private readonly IModulesRepository<ServerModuleBase> _serverModules;

        public DependenciesResolver(IModulesRepository<ClientModuleBase> clientModules, IModulesRepository<ServerModuleBase> serverModules)
        {
            _clientModules = clientModules;
            _serverModules = serverModules;
        }

        /// <summary>
        /// Check if server module can be resolved. (check all module dependencies)
        /// </summary>
        public virtual ServerModuleCheckDepResult CheckDependencies(ServerModuleBase module)
        {
            foreach (var moduleDependency in module.ServerDependencies)
            {
                if (!_serverModules.ContainsModule(moduleDependency))
                {
                }
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if client module can be resolved. (check all module dependencies)
        /// </summary>
        public virtual ClientModuleCheckDepResult CheckDependencies(ClientModuleBase module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if modules can be resolved. (check all module dependencies)
        /// </summary>
        public virtual ClientModuleCheckDepResult CheckDependencies(IEnumerable<IModule> modules)
        {
            throw new NotImplementedException();
        }
    }
}
