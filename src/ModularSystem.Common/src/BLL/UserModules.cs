using System.Collections.Generic;
using System.Linq;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Common.BLL
{
    /// <summary>
    /// Each user requires own set of modules. This class contains this list for each user.
    /// </summary>
    public class UserModules
    {
        private readonly Modules _modules;
        private readonly IUserModulesRepository _userModules;

        public UserModules(Modules modules, IUserModulesRepository userModules)
        {
            _modules = modules;
            _userModules = userModules;
        }

        /// <summary>
        /// Add module as required for specifed user
        /// </summary>
        public void AddModule(string userId, ModuleIdentity module)
        {
            var t = _modules.GetModule(module)?.ModuleInfo;
            if (t == null)
                throw new ModuleMissedException(module);
            var depRes = ModulesHelper.CheckDependencies(t, GetModules(userId) ?? new ModuleIdentity[0]);
            if (depRes.IsCheckSuccess)
            {
                _userModules.AddModule(userId, module);
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
            var ordered = ModulesHelper.OrderModules(modules.Select(x => _modules.GetModule(x).ModuleInfo));

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
                _userModules.GetModules(userId).Select(x => _modules.GetModule(x).ModuleInfo))
                .ToArray();
            if (dependent.Any())
                throw new ModuleIsRequiredException(module, dependent);
            _userModules.RemoveModule(userId, module);
        }

        /// <summary>
        /// Remove list of modules from user requirments. Order of modules is not important 
        /// (it will be sort automaticaly with considering of dependencies)
        /// </summary>
        public void RemoveModules(string userId, IEnumerable<ModuleIdentity> modules)
        {
            var ordered = ModulesHelper.OrderModules(modules.Select(x => _modules.GetModule(x).ModuleInfo)).Reverse();

            foreach (var moduleInfo in ordered)
            {
                RemoveModule(userId, moduleInfo.ModuleIdentity);
            }
        }

        public IEnumerable<ModuleIdentity> GetModules(string userId)
        {
            return _userModules.GetModules(userId);
        }
    }
}
