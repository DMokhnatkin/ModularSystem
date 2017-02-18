using System;
using System.Collections.Generic;
using System.Linq;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Common
{
    public static class ModulesHelper
    {
        /// <summary>
        /// Order list of module infos in right order. (in case of dependencies)
        /// i.e. module[i] can require module[i+k]. In this case module[i+k] must be before module[i] in list.
        /// </summary>
        /// <example>
        /// { module1, module2 }. Module1 requires module2 as dependency. 
        /// In this case { module2, module1 } will be returned. 
        /// </example>
        public static IEnumerable<ModuleInfo> OrderModules(IEnumerable<ModuleInfo> modules)
        {
            // We will count number of links of module to other modules in list.
            // Right order - ascending order of this values
            var moduleInfos = modules as ModuleInfo[] ?? modules.ToArray();
            Dictionary<ModuleIdentity, int> linkesCt = moduleInfos.ToDictionary(x => x.ModuleIdentity, x => 0);
            foreach (var moduleInfo in moduleInfos)
            {
                foreach (var dependency in moduleInfo.Dependencies)
                {
                    // We need increase value only if dependency module is in source modules list.
                    if (linkesCt.ContainsKey(dependency))
                        linkesCt[moduleInfo.ModuleIdentity]++;
                }
            }
            return moduleInfos.OrderBy(x => linkesCt[x.ModuleIdentity]);
        }

        /// <summary>
        /// Check dependencies of module in given list.
        /// I.e. is given modules list is enough to install module.
        /// </summary>
        public static ICheckDependenciesResult CheckDependencies(ModuleInfo module, IEnumerable<ModuleIdentity> modules)
        {
            HashSet<ModuleIdentity> modulesSet = modules as HashSet<ModuleIdentity> ?? new HashSet<ModuleIdentity>(modules);
            Dictionary<ModuleIdentity, Exception> missedModules = new Dictionary<ModuleIdentity, Exception>();
            foreach (var moduleDependency in module.Dependencies)
            {
                if (!modulesSet.Contains(moduleDependency))
                    missedModules.Add(moduleDependency, new ModuleMissedException(moduleDependency));
            }
            return new CheckDependenciesResult(module.ModuleIdentity, missedModules);
        }

        /// <summary>
        /// Calc count of dependent modules for given module 
        /// </summary>
        public static int CalcCountOfDependent(ModuleIdentity module, IEnumerable<ModuleInfo> modules)
        {
            int res = 0;
            foreach (var moduleInfo in modules)
            {
                foreach (var dependency in moduleInfo.Dependencies)
                {
                    if (dependency.Equals(module))
                        res++;
                }
            }
            return res;
        }
    }
}
