using System.IO;

namespace ModularSystem.Common.PackedModules
{
    public interface IPacked
    {
        Stream OpenWriteStream();

        Stream OpenReadStream();

        Stream OpenEditStream();

        byte[] ExtractBytes();

        void CopyTo(Stream stream);
    }
}
