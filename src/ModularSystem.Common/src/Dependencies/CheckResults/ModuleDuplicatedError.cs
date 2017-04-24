using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Module need to be installed, but it is alredy installed.
    /// </summary>
    public class ModuleDuplicatedError : ICheckResult
    {
        public ModuleDuplicatedError(IModule sourceModule)
        {
            SourceModule = sourceModule;
        }

        /// <summary>
        /// For this module check was done.
        /// </summary>
        public IModule SourceModule { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        public string GetMessage()
        {
            return $"Module {SourceModule} is already in repository";
        }
    }
}
