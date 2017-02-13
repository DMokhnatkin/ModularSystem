using System.Collections.Generic;
using ModularSystem.Common;
using ModularSystem.Communication.Data;

namespace ModularSystem.Server.Models
{
    public class DownloadModulesRequest : IDownloadModulesRequest
    {
        /// <inheritdoc />
        public IEnumerable<ModuleIdentity> ModuleIdentities { get; }

        public DownloadModulesRequest(IEnumerable<ModuleIdentity> moduleIdentities)
        {
            ModuleIdentities = moduleIdentities;
        }
    }
}
