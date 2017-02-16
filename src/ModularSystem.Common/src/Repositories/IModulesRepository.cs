using System.Collections.Generic;

namespace ModularSystem.Common.Repositories
{
    public interface IModulesRepository
    {
        void RegisterModule(IModule module);
        void UnregisterModule(ModuleIdentity moduleIdentity);

        /// <summary>
        /// Returns module by it's identity
        /// </summary>
        IModule GetModule(ModuleIdentity moduleIdentity);

        /// <summary>
        /// Check if module can be used. (check all module dependencies)
        /// </summary>
        ICheckDependenciesResult CheckDependencies(ModuleInfo moduleInfo);

        /// <summary>
        /// Get all dependent modules
        /// </summary>
        IEnumerable<ModuleIdentity> GetDependent(ModuleIdentity moduleInfo);

        /// <summary>
        /// Register list of modules.
        /// This method will try to register modules in right order.
        /// </summary>
        void RegisterModules(IEnumerable<IModule> modules);

        /// <summary>
        /// Unregister list of modules.
        /// This method will try to unregister modules in right order.
        /// </summary>
        void UnregisterModules(IEnumerable<ModuleIdentity> moduleIdentity);
    }
}
