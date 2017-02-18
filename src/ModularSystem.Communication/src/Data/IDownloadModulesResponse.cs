using System.Collections.Generic;
using ModularSystem.Communication.Data.Dto;

namespace ModularSystem.Communication.Data
{
    public interface IDownloadModulesResponse
    {
        IEnumerable<ModuleDto> Modules { get; }
    }
}
