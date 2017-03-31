using System.IO;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.PackedModules.Zip
{
    /// <summary>
    /// Represents collection of zip packed modules packed (batched) in one zip archive.
    /// </summary>
    public class FileBatchedModules : IPathStoredModule, IBatchedModules
    {
        /// <inheritdoc />
        public string Path { get; }

        /// <summary>
        /// Initialize FileBatchedModules from zip archive
        /// </summary>
        public FileBatchedModules(string path)
        {
            Path = path;
        }

        /// <inheritdoc />
        public Stream OpenStream()
        {
            return File.OpenRead(Path);
        }
    }
}
