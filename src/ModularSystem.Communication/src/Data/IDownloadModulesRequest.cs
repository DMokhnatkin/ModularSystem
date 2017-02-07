using System.Collections.Generic;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    public interface IDownloadModulesRequest
    {
        IEnumerable<ModuleIdentity> ModuleIdentities { get; }
    }
}
