using System.Linq;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Result of dependencies check for list of modules.
    /// </summary>
    public class CheckDependenciesResult : ICheckDependenciesResult
    {
        public CheckDependenciesResult(ClientModuleCheckDepResult[] clientModulesResult, ServerModuleCheckDepResult[] serverModulesResult, IModule[] sourceModules)
        {
            ClientModulesResult = clientModulesResult;
            ServerModulesResult = serverModulesResult;
            SourceModules = sourceModules;

            DependencyErrors = Result.Where(x => !x.IsSuccess).Select(x => x.DependencyErrors).Aggregate((a, b) => a.Concat(b).ToArray());
            IsSuccess = DependencyErrors.Length == 0;
        }

        public IModule[] SourceModules { get; }

        public ClientModuleCheckDepResult[] ClientModulesResult { get; }
        public ServerModuleCheckDepResult[] ServerModulesResult { get; }
        public ICheckDependenciesResult[] Result => ClientModulesResult.OfType<ICheckDependenciesResult>().Concat(ServerModulesResult).ToArray();

        /// <inheritdoc />
        public bool IsSuccess { get; }

        /// <inheritdoc />
        public IDependencyError<IModule, IModule>[] DependencyErrors { get; }
    }
}
