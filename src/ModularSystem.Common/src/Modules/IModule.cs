namespace ModularSystem.Common.Modules
{
    /// <summary>
    /// Base module interface. All modules implement it.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Identifier for module
        /// </summary>
        ModuleIdentity ModuleIdentity { get; }

        /// <summary>
        /// List of modules (all types i.e. server and client) which are required to be installed for this module.
        /// </summary>
        ModuleIdentity[] Dependencies { get; }
    }
}