using System;
using System.Collections.Generic;
using System.Linq;
using ModularSystem.Common.Exceptions;

namespace ModularSystem.Common.BLL
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

        /// <inheritdoc />
        public ModuleMissedException ToOneException()
        {
            return new ModuleMissedException(FailedModules.Select(x => x.Key));
        }

        public CheckDependenciesResult(ModuleIdentity sourceModule, Dictionary<ModuleIdentity, Exception> failedModules)
        {
            SourceModule = sourceModule;
            _failedModules = failedModules;
        }
    }
}
