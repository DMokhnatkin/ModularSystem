using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Occurs when dependecy is invalid. F.e. if server module has client module in dependecies. 
    /// </summary>
    public class InvalidDependency : ICheckResult
    {
        public InvalidDependency(IModule sourceModule, IModule requiredModule)
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
            return $"{SourceModule} can't require {RequiredModule}.";
        }
    }
}
