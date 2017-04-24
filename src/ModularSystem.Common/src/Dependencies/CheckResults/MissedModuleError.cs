using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Some module was required by other module but was missed.
    /// </summary>
    class MissedModuleError : ICheckResult
    {
        public MissedModuleError(IModule sourceModule, IModule requiredModule)
        {
            SourceModule = sourceModule;
            RequiredModule = requiredModule;
        }

        public IModule SourceModule { get; }

        public IModule RequiredModule { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        /// <inheritdoc />
        public string GetMessage()
        {
            return $"Module {RequiredModule.ModuleIdentity} was required by {SourceModule.ModuleIdentity} but was missed";
        }
    }
}
