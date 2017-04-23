using System;
using System.Collections;
using System.Collections.Generic;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Repositories
{
    public class MemoryModulesRepository<T> : IModulesRepository<T> where T : IModule
    {
        private Dictionary<ModuleIdentity, T> _modules = new Dictionary<ModuleIdentity, T>();

        /// <inheritdoc />
        public void AddModule(T module)
        {
            if (_modules.ContainsKey(module.ModuleIdentity))
                throw new ArgumentException($"Module {module.ModuleIdentity} is already registered");
            _modules.Add(module.ModuleIdentity, module);
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!_modules.ContainsKey(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} isn't registered");
            _modules.Remove(moduleIdentity);
        }

        /// <inheritdoc />
        public T GetModule(ModuleIdentity moduleIdentity)
        {
            T res;
            _modules.TryGetValue(moduleIdentity, out res);
            return res;
        }

        /// <inheritdoc />
        public bool ContainsModule(ModuleIdentity moduleIdentity)
        {
            return GetModule(moduleIdentity) != null;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _modules.Values.GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _modules.Values.GetEnumerator();
        }
    }
}
