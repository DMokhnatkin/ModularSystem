using System.Collections.Generic;

namespace ModularSystem.Common.Modules
{
    /// <summary>
    /// Base module interface. All modules implement it.
    /// </summary>
    public interface IModule
    {
        /// <see cref="ModuleIdentity"/>
        ModuleIdentity ModuleIdentity { get; }

        IEnumerable<ModuleIdentity> Dependencies { get; }
    }
}