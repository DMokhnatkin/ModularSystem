﻿using System;
using System.Collections.Generic;
using ModularSystem.Common;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Server.Repositories
{
    public class ModulesRepository : IModulesRepository
    {
        // PERFOMANCE: store in db?
        // PERFOMANCE: group by ModuleType, ModuleVersion or Name (now it isn't necessary)
        private Dictionary<ModuleIdentity, IModule> _modules = new Dictionary<ModuleIdentity, IModule>();

        /// <inheritdoc />
        public void RegisterModule(IModule module)
        {
            if (_modules.ContainsKey(module.ModuleInfo.ModuleIdentity))
                throw new ArgumentException($"Module {module.ModuleInfo.ModuleIdentity} is already registered");
            _modules.Add(module.ModuleInfo.ModuleIdentity, module);
        }

        /// <inheritdoc />
        public void UnregisterModule(ModuleIdentity moduleIdentity)
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
        public IEnumerable<ModuleIdentity> ResolveDependencies(IEnumerable<ModuleIdentity> moduleIdentities)
        {
            HashSet<ModuleIdentity> result = new HashSet<ModuleIdentity>();
            foreach (var moduleIdentity in moduleIdentities)
            {
                // If hashet contains moduleIdentity it will be just ignored
                result.Add(moduleIdentity);
            }
            return result;
        }
    }
}