using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    public interface IDependencyError<out TSourceModule, out TErrorModule>
        where TSourceModule : IModule
        where TErrorModule : IModule
    {
        TSourceModule SourceModule { get; }
        TErrorModule ErrorModule { get; }

        string ErrorMessage { get; }
    }
}
