using System.IO;

namespace ModularSystem.Common.PackedModules
{
    public interface IBatchedModules
    {
        Stream OpenStream();
    }
}
