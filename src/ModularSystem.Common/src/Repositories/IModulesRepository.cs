using System.Collections.Generic;

namespace ModularSystem.Common.Repositories
{
    public interface IModulesRepository
    {
        void RegisterModule(IModule module);
        void UnregisterModule(ModuleIdentity moduleIdentity);
        IModule GetModule(ModuleIdentity moduleIdentity);

        ICheckDependenciesResult CheckDependencies(ModuleInfo moduleInfo);
    }
}
