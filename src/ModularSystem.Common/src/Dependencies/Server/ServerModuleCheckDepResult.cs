using System.Linq;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Result of dependencies check for one server module
    /// </summary>
    public class ServerModuleCheckDepResult : ICheckDependenciesResult
    {
        public ServerModuleCheckDepResult(IDependencyError<ServerModuleBase, IModule>[] serverDependencyErrors, ClientModuleBase sourceModule)
        {
            ServerDependencyErrors = serverDependencyErrors;
            SourceModule = sourceModule;
            IsSuccess = DependencyErrors.Length == 0;
        }

        /// <summary>
        /// Module for which Dependency check was done.
        /// </summary>
        public ClientModuleBase SourceModule { get; }

        /// <summary>
        /// List of server modules which can't be resolved for SourceModule.
        /// </summary>
        public IDependencyError<ServerModuleBase, IModule>[] ServerDependencyErrors { get; }

        /// <inheritdoc />
        public IDependencyError<IModule, IModule>[] DependencyErrors => ServerDependencyErrors.OfType<IDependencyError<IModule, IModule>>().ToArray();

        /// <inheritdoc />
        public bool IsSuccess { get; }
    }
}
