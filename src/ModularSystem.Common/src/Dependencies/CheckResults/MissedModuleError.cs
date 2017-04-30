using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Some module was required by other module but was missed.
    /// </summary>
    public class MissedModuleError : ICheckResult
    {
        public MissedModuleError(IModule sourceModule, ModuleIdentity requiredModule, ModuleType requiredModuleType)
        {
            SourceModule = sourceModule;
            RequiredModule = requiredModule;
            RequiredModuleType = requiredModuleType;
        }

        public IModule SourceModule { get; }

        public ModuleIdentity RequiredModule { get; }

        public ModuleType RequiredModuleType { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        /// <inheritdoc />
        public virtual string GetMessage()
        {
            return $"Module {RequiredModule} was required by {SourceModule.ModuleIdentity} on {RequiredModuleType} side but was missed";
        }
    }
}
