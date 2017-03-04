using System;
using System.Collections;
using System.Collections.Generic;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Repositories
{
    public class MemoryModulesRepository : IModulesRepository
    {
        private Dictionary<ModuleIdentity, IPathModule> _modules = new Dictionary<ModuleIdentity, IPathModule>();

        /// <inheritdoc />
        public void AddModule(IPathModule packagedModule)
        {
            if (_modules.ContainsKey(packagedModule.ModuleInfo.ModuleIdentity))
                throw new ArgumentException($"Module {packagedModule.ModuleInfo.ModuleIdentity} is already registered");
            _modules.Add(packagedModule.ModuleInfo.ModuleIdentity, packagedModule);
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!_modules.ContainsKey(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} isn't registered");
            _modules.Remove(moduleIdentity);
        }

        /// <inheritdoc />
        public IPathModule GetModule(ModuleIdentity moduleIdentity)
        {
            IPathModule res;
            _modules.TryGetValue(moduleIdentity, out res);
            return res;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _modules.Values.GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<IPathModule> GetEnumerator()
        {
            return _modules.Values.GetEnumerator();
        }
    }
}
