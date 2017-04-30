using System.Linq;

namespace ModularSystem.Common.Modules
{
    public abstract class ClientModuleBase : IModule
    {
        protected ClientModuleBase(ModuleIdentity moduleIdentity, ModuleIdentity[] serverDependencies, ModuleIdentity[] clientDependencies)
        {
            ModuleIdentity = moduleIdentity;
            ServerDependencies = serverDependencies;
            ClientDependencies = clientDependencies;
        }

        /// <inheritdoc />
        public ModuleIdentity ModuleIdentity { get; }

        /// <inheritdoc />
        public ModuleType Type => ModuleType.Client;

        /// <summary>
        /// List of modules which are required to be installed on server for this module.
        /// </summary>
        public ModuleIdentity[] ServerDependencies { get; }

        /// <summary>
        /// List of modules which are required to be installed on client for this module.
        /// </summary>
        public ModuleIdentity[] ClientDependencies { get; }

        /// <inheritdoc />
        // Can be cached
        public ModuleIdentity[] Dependencies => ServerDependencies.Concat(ClientDependencies).ToArray();
    }
}
