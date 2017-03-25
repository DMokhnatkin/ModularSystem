using System;
using System.Collections.Generic;
using System.Linq;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Common.BLL
{
    public class RegisteredModules
    {
        private readonly IModulesRepository<ZipPackagedModule> _modulesRepository;
        private readonly IUserModulesRepository _userModulesRepository;

        public RegisteredModules(IModulesRepository<ZipPackagedModule> modulesRepository, IUserModulesRepository userModulesRepository)
        {
            _modulesRepository = modulesRepository;
            _userModulesRepository = userModulesRepository;
        }

        #region Modules
        public virtual void RegisterModule(ZipPackagedModule packagedModule)
        {
            var t = CheckDependencies(packagedModule);
            if (!t.IsCheckSuccess)
                throw t.ToOneException();
            _modulesRepository.AddModule(packagedModule);
        }

        /// <summary>
        /// Register list of modules.
        /// This method will try to register modules in right order.
        /// </summary>
        public virtual void RegisterModules(IEnumerable<ZipPackagedModule> modules)
        {
            var enumerable = modules as ZipPackagedModule[] ?? modules.ToArray();
            var identityToModule = enumerable.ToDictionary(x => x.ModuleIdentity, x => x); // Just for get IModule by ModuleIdentity
            var orderedModules = ModulesHelper.OrderModules(enumerable);
            foreach (var m in orderedModules)
            {
                RegisterModule(identityToModule[m.ModuleIdentity]);
            }
        }

        public virtual void UnregisterModule(ModuleIdentity moduleIdentity)
        {
            var t = GetDependent(moduleIdentity);
            var moduleIdentities = t as ModuleIdentity[] ?? t.ToArray();
            if (moduleIdentities.Any())
                throw new ModuleIsRequiredException(moduleIdentity, moduleIdentities);
            _modulesRepository.RemoveModule(moduleIdentity);
        }

        /// <summary>
        /// Unregister list of modules.
        /// This method will try to unregister modules in right order.
        /// </summary>
        public virtual void UnregisterModules(IEnumerable<ModuleIdentity> moduleIdentities)
        {
            var identities = moduleIdentities as ModuleIdentity[] ?? moduleIdentities.ToArray();
            var infos = identities.Select(GetModule);
            var orderedModules = ModulesHelper.OrderModules(infos).Reverse();
            foreach (var m in orderedModules)
            {
                UnregisterModule(m.ModuleIdentity);
            }
        }

        /// <summary>
        /// Returns module by it's identity
        /// </summary>
        public virtual ZipPackagedModule GetModule(ModuleIdentity moduleIdentity)
        {
            return _modulesRepository.GetModule(moduleIdentity);
        }

        /// <summary>
        /// Get all registered modules
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ZipPackagedModule> GetRegisteredModules()
        {
            return _modulesRepository;
        }

        /// <summary>
        /// Check if module can be registered. (check all module dependencies)
        /// </summary>
        public virtual ICheckDependenciesResult CheckDependencies(IModule moduleInfo)
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

        /// <summary>
        /// Get all dependent modules
        /// </summary>
        public virtual IEnumerable<ModuleIdentity> GetDependent(ModuleIdentity moduleInfo)
        {
            List<ModuleIdentity> res = new List<ModuleIdentity>();
            foreach (var module in _modulesRepository)
            {
                if (module.Dependencies.Contains(moduleInfo))
                    res.Add(module.ModuleIdentity);
            }
            return res;
        }
        #endregion

        #region User modules
        /// <summary>
        /// Add module as required for specifed user
        /// </summary>
        public void AddModule(string userId, string clientId, ModuleIdentity module)
        {
            var t = _modulesRepository.GetModule(module);
            if (t == null)
                throw new ModuleMissedException(module);
            var depRes = ModulesHelper.CheckDependencies(t, GetModuleIdentities(userId, clientId) ?? new ModuleIdentity[0]);
            if (depRes.IsCheckSuccess)
            {
                _userModulesRepository.AddModule(userId, clientId, module);
            }
            else
            {
                throw depRes.ToOneException();
            }
        }

        /// <summary>
        /// Add list of modules from user requirments. Order of modules is not important 
        /// (it will be sort automaticaly with considering of dependencies)
        /// </summary>
        public void AddModules(string userId, string clientId, IEnumerable<ModuleIdentity> modules)
        {
            var ordered = ModulesHelper.OrderModules(modules.Select(x => _modulesRepository.GetModule(x)));

            foreach (var moduleInfo in ordered)
            {
                AddModule(userId, clientId, moduleInfo.ModuleIdentity);
            }
        }

        /// <summary>
        /// Remove module from required for specifed user
        /// </summary>
        public void RemoveModule(string userId, string clientId, ModuleIdentity module)
        {
            var dependent = ModulesHelper.GetDependent(module,
                _userModulesRepository.GetModules(userId, clientId).Select(x => _modulesRepository.GetModule(x)))
                .ToArray();
            if (dependent.Any())
                throw new ModuleIsRequiredException(module, dependent);
            _userModulesRepository.RemoveModule(userId, clientId, module);
        }

        /// <summary>
        /// Remove list of modules from user requirments. Order of modules is not important 
        /// (it will be sort automaticaly with considering of dependencies)
        /// </summary>
        public void RemoveModules(string userId, string clientId, IEnumerable<ModuleIdentity> modules)
        {
            var ordered = ModulesHelper.OrderModules(modules.Select(x => _modulesRepository.GetModule(x))).Reverse();

            foreach (var moduleInfo in ordered)
            {
                RemoveModule(userId, clientId, moduleInfo.ModuleIdentity);
            }
        }

        public IEnumerable<ModuleIdentity> GetModuleIdentities(string userId, string clientId)
        {
            return _userModulesRepository.GetModules(userId, clientId);
        }

        public IEnumerable<ZipPackagedModule> GetModules(string userId, string clientId)
        {
            var res = new List<ZipPackagedModule>();
            foreach (var moduleIdentity in GetModuleIdentities(userId, clientId))
            {
                var t = GetModule(moduleIdentity);
                if (t == null)
                    throw new ModuleMissedException(moduleIdentity);
                res.Add(t);
            }
            return res;
        }
        #endregion
    }
}
