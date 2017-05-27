using System.Collections.Generic;
using ModularSystem.Common.PackedModules;

namespace ModularSystem.Common.Repositories.PackedModules
{
    public interface IPackedModulesRepository
    {
        IPackedModule AddModule(ModuleIdentity identity, IPackedModule module);

        IEnumerable<ModuleIdentity> GetIdentities();

        void RemoveModule(ModuleIdentity moduleIdentity);

        IPackedModule GetModule(ModuleIdentity moduleIdentity);

        bool IsModuleRegistered(ModuleIdentity moduleIdentity);
    }
}