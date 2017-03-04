using System.IO;

namespace ModularSystem.Common.Modules
{
    public interface IPackagedModule : IModule
    {
        Stream Data { get; }
    }
}
