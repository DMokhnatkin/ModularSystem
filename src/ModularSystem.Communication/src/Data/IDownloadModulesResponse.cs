using System.Collections.Generic;

namespace ModularSystem.Communication.Data
{
    public interface IDownloadModulesResponse
    {
        IEnumerable<ModuleDto> Modules { get; }
    }
}
