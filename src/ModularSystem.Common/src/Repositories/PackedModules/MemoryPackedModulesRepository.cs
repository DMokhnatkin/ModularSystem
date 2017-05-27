using System;
using System.Collections.Generic;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.Repositories.PackedModules
{
    public class MemoryPackedModulesRepository : IPackedModulesRepository
    {
        private Dictionary<ModuleIdentity, IPackedModule> _modules = new Dictionary<ModuleIdentity, IPackedModule>();

        /// <inheritdoc />
        public IPackedModule AddModule(ModuleIdentity identity, IPackedModule packed)
        {
            if (IsModuleRegistered(identity))
                throw new ArgumentException($"Module {packed.ModuleIdentity} is already registered");
            
            var newModulePack = new MemoryPackedModule(packed.ExtractBytes());
            _modules.Add(packed.ModuleIdentity, newModulePack);
            return newModulePack;
        }

        /// <inheritdoc />
        public IEnumerable<ModuleIdentity> GetIdentities()
        {
            return _modules.Keys;
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!_modules.ContainsKey(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} isn't registered");
            _modules.Remove(moduleIdentity);
        }

        /// <inheritdoc />
        public IPackedModule GetModule(ModuleIdentity moduleIdentity)
        {
            IPackedModule res;
            _modules.TryGetValue(moduleIdentity, out res);
            return res;
        }

        /// <inheritdoc />
        public bool IsModuleRegistered(ModuleIdentity moduleIdentity)
        {
            return _modules.ContainsKey(moduleIdentity);
        }
    }
}
