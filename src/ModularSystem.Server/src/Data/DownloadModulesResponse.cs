using System.Collections.Generic;
using ModularSystem.Communication.Data;

namespace ModularSystem.Server.Data
{
    public class DownloadModulesResponse : IDownloadModulesResponse
    {
        public DownloadModulesResponse(IEnumerable<ModuleDto> modules)
        {
            Modules = modules;
        }

        /// <inheritdoc />
        public IEnumerable<ModuleDto> Modules { get; }
    }
}
