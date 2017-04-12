using System.IO;

namespace ModularSystem.Common.PackedModules
{
    public interface IPackedModuleV2
    {
        Stream OpenWriteStream();

        Stream OpenReadStream();

        Stream OpenEditStream();
    }
}
