using System.Collections.Generic;
using System.Linq;

namespace ModularSystem.Common.Modules
{
    public class ModulesGroup : IModulesGroup
    {
        private readonly Dictionary<ModuleIdentity, IModuleInfo> _clientModules;
        private readonly Dictionary<ModuleIdentity, IModuleInfo> _serverModules;

        public ModulesGroup(IEnumerable<ClientModuleInfoBase> clientModules, IEnumerable<ServerModuleInfoBase> serverModules)
        {
            _clientModules = clientModules.ToDictionary(x => x.ModuleIdentity, y => (IModuleInfo)y);
            _serverModules = serverModules.ToDictionary(x => x.ModuleIdentity, y => (IModuleInfo)y);
        }

        public ModulesGroup(IEnumerable<IModuleInfo> modules)
        {
            _clientModules = modules.Where(x => x.Type == ModuleType.Client).ToDictionary(x => x.ModuleIdentity, y => y);
            _serverModules = modules.Where(x => x.Type == ModuleType.Server).ToDictionary(x => x.ModuleIdentity, y => y);
        }

        /// <inheritdoc />
        public IEnumerable<IModuleInfo> ClientModules => _clientModules.Values;

        /// <inheritdoc />
        public IEnumerable<IModuleInfo> ServerModules => _serverModules.Values;

        /// <inheritdoc />
        public bool ContainsServerModule(ModuleIdentity module)
        {
            return _serverModules.ContainsKey(module);
        }

        /// <inheritdoc />
        public bool ContainsClientModule(ModuleIdentity module)
        {
            return _clientModules.ContainsKey(module);
        }
    }
}
