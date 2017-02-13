
using System.Collections.Generic;
using ModularSystem.Communication.Data;

namespace ModularSystem.Server.Models
{
    public class DownloadModulesResponse : IDownloadModulesResponse
    {
        /// <inheritdoc />
        public IEnumerable<ModuleDto> Modules { get; }

        public DownloadModulesResponse(IEnumerable<ModuleDto> modules)
        {
            Modules = modules;
        }
    }
}
