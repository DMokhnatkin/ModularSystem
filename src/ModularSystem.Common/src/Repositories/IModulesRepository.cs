using System.Collections.Generic;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Repositories
{
    public interface IModulesRepository : IEnumerable<IPackagedModule>
    {
        void AddModule(IPackagedModule packagedModule);
        void RemoveModule(ModuleIdentity moduleIdentity);

        /// <summary>
        /// Returns module by it's identity
        /// </summary>
        IPackagedModule GetModule(ModuleIdentity moduleIdentity);
    }
}
