using System;
using System.Collections.Generic;

namespace ModularSystem.Common
{
    public interface ICheckDependenciesResult
    {
        ModuleIdentity SourceModule { get; }

        IEnumerable<KeyValuePair<ModuleIdentity, Exception>> FailedModules { get; }

        bool IsCheckSuccess { get; }
    }
}
