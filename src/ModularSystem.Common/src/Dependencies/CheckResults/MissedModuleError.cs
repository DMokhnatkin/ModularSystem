using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Some module was required by other module but was missed.
    /// </summary>
    public class MissedModuleError : ICheckResult
    {
        public MissedModuleError(IModuleInfo sourceModuleInfo, ModuleIdentity requiredModule, ModuleType requiredModuleType)
        {
            SourceModuleInfo = sourceModuleInfo;
            RequiredModule = requiredModule;
            RequiredModuleType = requiredModuleType;
        }

        public IModuleInfo SourceModuleInfo { get; }

        public ModuleIdentity RequiredModule { get; }

        public ModuleType RequiredModuleType { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        /// <inheritdoc />
        public virtual string GetMessage()
        {
            return $"Module {RequiredModule} was required by {SourceModuleInfo.ModuleIdentity} on {RequiredModuleType} side but was missed";
        }
    }
}
