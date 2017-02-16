using System.Collections.Generic;
using System.Linq;

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
    }
}
