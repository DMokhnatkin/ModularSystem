namespace ModularSystem.Common.Modules
{
    public abstract class ServerModuleBase : IModule
    {
        protected ServerModuleBase(ModuleIdentity moduleIdentity, ModuleIdentity[] serverDependencies)
        {
            ModuleIdentity = moduleIdentity;
            ServerDependencies = serverDependencies;
        }

        /// <inheritdoc />
        public ModuleIdentity ModuleIdentity { get; }

        /// <inheritdoc />
        public ModuleType Type => ModuleType.Server;

        /// <summary>
        /// List of modules which are required to be installed on server for this module.
        /// </summary>
        public ModuleIdentity[] ServerDependencies { get; }

        /// <inheritdoc />
        public ModuleIdentity[] Dependencies => ServerDependencies;
    }
}
