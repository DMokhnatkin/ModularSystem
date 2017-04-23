using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    class MissedModuleError<TSourceModule, TErrorModule> : IDependencyError<TSourceModule, TErrorModule> 
        where TSourceModule : IModule
        where TErrorModule : IModule
    {
        public MissedModuleError(TSourceModule sourceModule, TErrorModule errorModule)
        {
            SourceModule = sourceModule;
            ErrorModule = errorModule;
        }

        /// <inheritdoc />
        public TSourceModule SourceModule { get; }

        /// <inheritdoc />
        public TErrorModule ErrorModule { get; }

        /// <inheritdoc />
        public string ErrorMessage => $"Module {ErrorModule.ModuleIdentity} was required by {SourceModule.ModuleIdentity} but was missed";
    }
}
