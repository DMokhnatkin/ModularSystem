using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Wpf.Modules
{
    public class InstalledModule : IPathModule
    {
        /// <inheritdoc />
        public ModuleInfo ModuleInfo { get; set; }

        /// <inheritdoc />
        public string Path { get; set; }

        /// <inheritdoc />
        public void InitializeFromPath(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
