using System;
using System.Collections.Generic;
using System.Linq;
using ModularSystem.Common.Exceptions;

namespace ModularSystem.Common.Repositories
{
    public class ModulesRepository : IModulesRepository
    {
        // PERFOMANCE: store in db?
        // PERFOMANCE: group by ModuleType, ModuleVersion or Name (now it isn't necessary)
        // PERFOMANCE: GetDependent is very long. Can be improved by change collection type of dependecies in module info. Or by adding cache.
        private Dictionary<ModuleIdentity, IModule> _modules = new Dictionary<ModuleIdentity, IModule>();

        /// <inheritdoc />
        public void RegisterModule(IModule module)
        {
            if (_modules.ContainsKey(module.ModuleInfo.ModuleIdentity))
                throw new ArgumentException($"Module {module.ModuleInfo.ModuleIdentity} is already registered");
            var t = CheckDependencies(module.ModuleInfo);
            if (!t.IsCheckSuccess)
                throw new ArgumentException($"CheckDependencies for {module.ModuleInfo} failed.", t.ToOneException());
            _modules.Add(module.ModuleInfo.ModuleIdentity, module);
        }

        /// <inheritdoc />
        /// This method will try to register modules in right order
        public void RegisterModules(IEnumerable<IModule> modules)
        {
            // TODO: register in right order. modules[i] can require modules[i+k]. In this case swap them 
            foreach (var m in modules)
            {
                RegisterModule(m);
            }
        }

        /// <inheritdoc />
        public void UnregisterModule(ModuleIdentity moduleIdentity)
        {
            if (!_modules.ContainsKey(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} isn't registered");
            var t = GetDependent(moduleIdentity);
            var moduleIdentities = t as ModuleIdentity[] ?? t.ToArray();
            if (moduleIdentities.Any())
                throw new ModuleIsRequiredException(moduleIdentity, moduleIdentities);
            _modules.Remove(moduleIdentity);
        }

        /// <inheritdoc />
        public void UnregisterModules(IEnumerable<ModuleIdentity> moduleIdentities)
        {
            // TODO: unregister in right order. moduleIdentities[i] can require moduleIdentities[i-k]. In this case swap them 
            foreach (var m in moduleIdentities)
            {
                UnregisterModule(m);
            }
        }

        /// <inheritdoc />
        public IModule GetModule(ModuleIdentity moduleIdentity)
        {
            IModule res;
            _modules.TryGetValue(moduleIdentity, out res);
            return res;
        }

        /// <inheritdoc />
        public ICheckDependenciesResult CheckDependencies(ModuleInfo moduleInfo)
        {
            Dictionary<ModuleIdentity, Exception> failed = new Dictionary<ModuleIdentity, Exception>();
            foreach (var dependency in moduleInfo.Dependencies)
            {
                var module = GetModule(dependency);
                if (module == null)
                    failed.Add(dependency, new ModuleMissedException(dependency));
            }
            return new CheckDependenciesResult(moduleInfo.ModuleIdentity, failed);
        }

        /// <inheritdoc />
        public IEnumerable<ModuleIdentity> GetDependent(ModuleIdentity moduleInfo)
        {
            List<ModuleIdentity> res = new List<ModuleIdentity>();
            foreach (var module in _modules.Values)
            {
                if (module.ModuleInfo.Dependencies.Contains(moduleInfo))
                    res.Add(module.ModuleInfo.ModuleIdentity);
            }
            return res;
        }
    }
}
