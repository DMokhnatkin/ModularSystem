using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularSystem.Common.Exceptions
{
    public class ModuleIsRequiredException : ArgumentException
    {
        public ModuleIsRequiredException(ModuleIdentity module, IEnumerable<ModuleIdentity> dependentModules)
        {
            ModuleIdentity = module;
            DependentModules = dependentModules;
        }

        public IEnumerable<ModuleIdentity> DependentModules { get; }

        public ModuleIdentity ModuleIdentity { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Module {ModuleIdentity} is required for {DependentModules.Select(x => x.ToString() + "; ")}.";
        }
    }
}
