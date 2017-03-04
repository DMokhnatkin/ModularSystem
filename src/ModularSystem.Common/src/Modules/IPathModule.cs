namespace ModularSystem.Common.Modules
{
    /// <summary>
    /// Represents modules which has related file or directory.
    /// F.e. it can be module packaged in zip archive.
    /// Another example is module which was unzipped in some directory and now this directory contains this module.
    /// </summary>
    public interface IPathModule : IModule
    {
        /// <summary>
        /// Path to file or directory which contains module.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Initialize IPathModule instance using file ot directory.
        /// </summary>
        void InitializeFromPath(string path);
    }
}