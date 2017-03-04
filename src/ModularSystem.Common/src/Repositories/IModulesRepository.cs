using System.Collections.Generic;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Repositories
{
    public interface IModulesRepository : IEnumerable<IPathModule>
    {
        void AddModule(IPathModule packagedModule);
        void RemoveModule(ModuleIdentity moduleIdentity);

        /// <summary>
        /// Returns module by it's identity
        /// </summary>
        IPathModule GetModule(ModuleIdentity moduleIdentity);
    }
}
