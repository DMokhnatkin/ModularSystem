using System;

namespace ModularSystem.Common.Modules
{
    /// <summary>
    /// Unique identifier for module.
    /// </summary>
    public interface IModuleIdentity
    {
        /// <summary>
        /// Name of module.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Version of module.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Type of module
        /// </summary>
        ModuleType Type { get; }
    }
}
