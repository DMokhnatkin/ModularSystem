using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ModularSystem.Common.Repositories
{
    public class InMemoryModulesRepository : IModulesRepository
    {
        // PERFOMANCE: store in db?
        // PERFOMANCE: group by ModuleType, ModuleVersion or Name (now it isn't necessary)
        // PERFOMANCE: GetDependent is very long. Can be improved by change collection type of dependecies in module info. Or by adding cache.
        private Dictionary<ModuleIdentity, IModule> _modules = new Dictionary<ModuleIdentity, IModule>();

        /// <inheritdoc />
        public void AddModule(IModule module)
        {
            if (_modules.ContainsKey(module.ModuleInfo.ModuleIdentity))
                throw new ArgumentException($"Module {module.ModuleInfo.ModuleIdentity} is already registered");
            _modules.Add(module.ModuleInfo.ModuleIdentity, module);
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!_modules.ContainsKey(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} isn't registered");
            _modules.Remove(moduleIdentity);
        }

        /// <inheritdoc />
        public IModule GetModule(ModuleIdentity moduleIdentity)
        {
            IModule res;
            _modules.TryGetValue(moduleIdentity, out res);
            return res;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _modules.Values.GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<IModule> GetEnumerator()
        {
            return _modules.Values.GetEnumerator();
        }
    }
}
