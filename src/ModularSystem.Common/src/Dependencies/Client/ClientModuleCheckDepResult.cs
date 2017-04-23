using System.Linq;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Result of dependencies check for one client module
    /// </summary>
    public class ClientModuleCheckDepResult : ICheckDependenciesResult
    {
        public ClientModuleCheckDepResult(ClientModuleBase sourceModule, IDependencyError<ClientModuleBase, IModule>[] clientDependencyErrors, IDependencyError<ServerModuleBase, IModule>[] serverDependencyErrors)
        {
            ClientDependencyErrors = clientDependencyErrors;
            ServerDependencyErrors = serverDependencyErrors;
            SourceModule = sourceModule;
            IsSuccess = DependencyErrors.Length == 0;
        }

        /// <summary>
        /// Module for which Dependency check was done.
        /// </summary>
        public ClientModuleBase SourceModule { get; }
        
        /// <summary>
        /// List of client modules which can't be resolved for SourceModule.
        /// </summary>
        public IDependencyError<ClientModuleBase, IModule>[] ClientDependencyErrors { get; }

        /// <summary>
        /// List of server modules which can't be resolved for SourceModule.
        /// </summary>
        public IDependencyError<ServerModuleBase, IModule>[] ServerDependencyErrors { get; }

        /// <inheritdoc />
        public IDependencyError<IModule, IModule>[] DependencyErrors => ClientDependencyErrors.OfType<IDependencyError<IModule, IModule>>().Concat(ServerDependencyErrors).ToArray();

        /// <inheritdoc />
        public bool IsSuccess { get; }
    }
}
