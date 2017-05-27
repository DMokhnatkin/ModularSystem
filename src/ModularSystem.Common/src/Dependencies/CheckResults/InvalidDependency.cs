using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Occurs when dependecy is invalid. F.e. if server module has client module in dependecies. 
    /// </summary>
    public class InvalidDependency : ICheckResult
    {
        public InvalidDependency(IModuleInfo sourceModuleInfo, IModuleInfo requiredModuleInfo)
        {
            SourceModuleInfo = sourceModuleInfo;
            RequiredModuleInfo = requiredModuleInfo;
        }

        public IModuleInfo SourceModuleInfo { get; }

        public IModuleInfo RequiredModuleInfo { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        /// <inheritdoc />
        public string GetMessage()
        {
            return $"{SourceModuleInfo} can't require {RequiredModuleInfo}.";
        }
    }
}
