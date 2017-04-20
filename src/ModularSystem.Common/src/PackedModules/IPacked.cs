using System.IO;

namespace ModularSystem.Common.PackedModules
{
    public interface IPacked
    {
        Stream OpenWriteStream();

        Stream OpenReadStream();

        Stream OpenEditStream();
    }
}
