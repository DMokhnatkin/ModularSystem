using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularSystem.Common.Exceptions
{
    public class ModuleIsRequiredException : ArgumentException
    {
        public ModuleIsRequiredException(ModuleIdentity module, IEnumerable<ModuleIdentity> dependentModules) : 
            base($"Module {module} is required for {string.Concat(dependentModules.Select(x => x.ToString() + "; "))}.")
        {
            ModuleIdentity = module;
            DependentModules = dependentModules;
        }

        public IEnumerable<ModuleIdentity> DependentModules { get; }

        public ModuleIdentity ModuleIdentity { get; }
    }
}
