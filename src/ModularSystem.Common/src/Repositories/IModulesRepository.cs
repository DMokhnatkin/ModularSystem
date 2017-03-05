using System.Collections.Generic;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Repositories
{
    public interface IModulesRepository<T> : IEnumerable<T> where T : IModule
    {
        void AddModule(T module);
        void RemoveModule(ModuleIdentity moduleIdentity);

        /// <summary>
        /// Returns module by it's identity
        /// </summary>
        T GetModule(ModuleIdentity moduleIdentity);
    }
}
