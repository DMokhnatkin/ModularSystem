namespace ModularSystem.Common.Modules
{
    /// <summary>
    /// Info about module (such as identity and dependencies)
    /// </summary>
    public interface IModuleInfo
    {
        /// <summary>
        /// Identifier for module
        /// </summary>
        IModuleIdentity ModuleIdentity { get; }

        /// <summary>
        /// List of modules (all types i.e. server and client) which are required to be installed for this module.
        /// </summary>
        IModuleIdentity[] Dependencies { get; }
    }
}