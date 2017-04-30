using System.Collections.Generic;

namespace ModularSystem.Common.Modules
{
    public interface IModulesGroup
    {
        IEnumerable<ClientModuleBase> ClientModules { get; }
        IEnumerable<ServerModuleBase> ServerModules { get; }

        bool ContainsServerModule(ModuleIdentity module);
        bool ContainsClientModule(ModuleIdentity module);
    }
}
