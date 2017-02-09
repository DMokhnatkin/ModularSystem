using System;
using System.Collections.Generic;
using ModularSystem.Common;

namespace ModularSystem.Server.Repositories
{
    public class CheckDependenciesResult : ICheckDependenciesResult
    {
        private readonly Dictionary<ModuleIdentity, Exception> _failedModules;

        /// <inheritdoc />
        public ModuleIdentity SourceModule { get; }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<ModuleIdentity, Exception>> FailedModules => _failedModules;

        /// <inheritdoc />
        public bool IsCheckSuccess => _failedModules?.Count == 0;

        public CheckDependenciesResult(ModuleIdentity sourceModule, Dictionary<ModuleIdentity, Exception> failedModules)
        {
            SourceModule = sourceModule;
            _failedModules = failedModules;
        }
    }
}
