using System.Collections.Generic;

namespace ModularSystem.Common.Repositories
{
    public interface IModulesRepository : IEnumerable<IModule>
    {
        void AddModule(IModule module);
        void RemoveModule(ModuleIdentity moduleIdentity);

        /// <summary>
        /// Returns module by it's identity
        /// </summary>
        IModule GetModule(ModuleIdentity moduleIdentity);
    }
}
