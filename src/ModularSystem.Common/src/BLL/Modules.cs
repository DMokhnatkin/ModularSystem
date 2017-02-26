using System;
using System.Collections.Generic;
using System.Linq;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Common.BLL
{
    public class Modules
    {
        private readonly IModulesRepository _modulesRepository;
        private readonly IUserModulesRepository _userModulesRepository;

        public Modules(IModulesRepository modulesRepository, IUserModulesRepository userModulesRepository)
        {
            _modulesRepository = modulesRepository;
            _userModulesRepository = userModulesRepository;
        }

        #region Modules
        public virtual void RegisterModule(IModule module)
        {
            var t = CheckDependencies(module.ModuleInfo);
            if (!t.IsCheckSuccess)
                throw t.ToOneException();
            _modulesRepository.AddModule(module);
        }

        /// <summary>
        /// Register list of modules.
        /// This method will try to register modules in right order.
        /// </summary>
        public virtual void RegisterModules(IEnumerable<IModule> modules)
        {
            var enumerable = modules as IModule[] ?? modules.ToArray();
            var identityToModule = enumerable.ToDictionary(x => x.ModuleInfo.ModuleIdentity, x => x); // Just for get IModule by ModuleIdentity
            var orderedModules = ModulesHelper.OrderModules(enumerable.Select(x => x.ModuleInfo));
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
            var infos = identities.Select(x => GetModule(x).ModuleInfo);
            var orderedModules = ModulesHelper.OrderModules(infos).Reverse();
            foreach (var m in orderedModules)
            {
                UnregisterModule(m.ModuleIdentity);
            }
        }

        /// <summary>
        /// Returns module by it's identity
        /// </summary>
        public virtual IModule GetModule(ModuleIdentity moduleIdentity)
        {
            return _modulesRepository.GetModule(moduleIdentity);
        }

        /// <summary>
        /// Get all registered modules
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IModule> GetRegisteredModules()
        {
            return _modulesRepository;
        }

        /// <summary>
        /// Check if module can be registered. (check all module dependencies)
        /// </summary>
        public virtual ICheckDependenciesResult CheckDependencies(ModuleInfo moduleInfo)
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
                if (module.ModuleInfo.Dependencies.Contains(moduleInfo))
                    res.Add(module.ModuleInfo.ModuleIdentity);
            }
            return res;
        }
        #endregion

        #region User modules
        /// <summary>
        /// Add module as required for specifed user
        /// </summary>
        public void AddModule(string userId, ModuleIdentity module)
        {
            var t = _modulesRepository.GetModule(module)?.ModuleInfo;
            if (t == null)
                throw new ModuleMissedException(module);
            var depRes = ModulesHelper.CheckDependencies(t, GetModuleIdentities(userId) ?? new ModuleIdentity[0]);
            if (depRes.IsCheckSuccess)
            {
                _userModulesRepository.AddModule(userId, module);
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
        public void AddModules(string userId, IEnumerable<ModuleIdentity> modules)
        {
            var ordered = ModulesHelper.OrderModules(modules.Select(x => _modulesRepository.GetModule(x)?.ModuleInfo));

            foreach (var moduleInfo in ordered)
            {
                AddModule(userId, moduleInfo.ModuleIdentity);
            }
        }

        /// <summary>
        /// Remove module from required for specifed user
        /// </summary>
        public void RemoveModule(string userId, ModuleIdentity module)
        {
            var dependent = ModulesHelper.GetDependent(module,
                _userModulesRepository.GetModules(userId).Select(x => _modulesRepository.GetModule(x).ModuleInfo))
                .ToArray();
            if (dependent.Any())
                throw new ModuleIsRequiredException(module, dependent);
            _userModulesRepository.RemoveModule(userId, module);
        }

        /// <summary>
        /// Remove list of modules from user requirments. Order of modules is not important 
        /// (it will be sort automaticaly with considering of dependencies)
        /// </summary>
        public void RemoveModules(string userId, IEnumerable<ModuleIdentity> modules)
        {
            var ordered = ModulesHelper.OrderModules(modules.Select(x => _modulesRepository.GetModule(x).ModuleInfo)).Reverse();

            foreach (var moduleInfo in ordered)
            {
                RemoveModule(userId, moduleInfo.ModuleIdentity);
            }
        }

        public IEnumerable<ModuleIdentity> GetModuleIdentities(string userId)
        {
            return _userModulesRepository.GetModules(userId);
        }

        public IEnumerable<IModule> GetModules(string userId)
        {
            return GetModuleIdentities(userId).Select(GetModule);
        }
        #endregion
    }
}
