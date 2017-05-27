using System.Collections.Generic;

namespace ModularSystem.Common.Modules
{
    public interface IModulesGroup
    {
        IEnumerable<IModuleInfo> ClientModules { get; }
        IEnumerable<IModuleInfo> ServerModules { get; }

        bool ContainsServerModule(ModuleIdentity module);
        bool ContainsClientModule(ModuleIdentity module);
    }
}
