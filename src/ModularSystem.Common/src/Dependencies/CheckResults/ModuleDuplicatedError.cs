using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Module need to be installed, but it is alredy installed.
    /// </summary>
    public class ModuleDuplicatedError : ICheckResult
    {
        public ModuleDuplicatedError(IModuleInfo sourceModuleInfo)
        {
            SourceModuleInfo = sourceModuleInfo;
        }

        /// <summary>
        /// For this module check was done.
        /// </summary>
        public IModuleInfo SourceModuleInfo { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        public string GetMessage()
        {
            return $"Module {SourceModuleInfo} is already in repository";
        }
    }
}
