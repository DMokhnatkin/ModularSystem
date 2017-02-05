using System;

namespace ModularSystem.Common
{
    public interface IModule : IEquatable<IModule>
    {
        ModuleInfo ModuleInfo { get; }

        string Path { get; }
    }
}
