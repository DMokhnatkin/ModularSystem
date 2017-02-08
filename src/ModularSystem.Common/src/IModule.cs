using System.IO;

namespace ModularSystem.Common
{
    public interface IModule
    {
        ModuleInfo ModuleInfo { get; }

        Stream Data { get; }
    }
}
