
using System.Collections.Generic;
using ModularSystem.Communication.Data;
using ModularSystem.Communication.Data.Dto;

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
