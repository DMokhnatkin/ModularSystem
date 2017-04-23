using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    public interface ICheckDependenciesResult
    {
        bool IsSuccess { get; }

        /// <summary>
        /// List of all (server and client) modules which can't be resolved.
        /// </summary>
        IDependencyError<IModule, IModule>[] DependencyErrors { get; }
    }
}
