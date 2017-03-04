namespace ModularSystem.Common
{
    /// <summary>
    /// Information about module. (name, dependencies, ...)
    /// </summary>
    public class ModuleInfo
    {
        /// <summary>
        /// Identifier for module
        /// </summary>
        public ModuleIdentity ModuleIdentity { get; }

        /// <summary>
        /// List of modules which are required to be installed for this module.
        /// </summary>
        public ModuleIdentity[] Dependencies { get; }

        public ModuleInfo(ModuleIdentity identity, ModuleIdentity[] dependencies)
        {
            ModuleIdentity = identity;
            Dependencies = dependencies;
        }
    }
}
