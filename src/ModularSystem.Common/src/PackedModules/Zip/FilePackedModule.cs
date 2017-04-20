using System.IO;
using System.Linq;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class FilePackedModule : IZipPackedModule
    {
        public string FilePath { get; }

        public FilePackedModule(string filePath)
        {
            FilePath = filePath;
        }

        /// <inheritdoc />
        public Stream OpenWriteStream()
        {
            return File.OpenWrite(FilePath);
        }

        /// <inheritdoc />
        public Stream OpenReadStream()
        {
            return File.OpenRead(FilePath);
        }

        /// <inheritdoc />
        public Stream OpenEditStream()
        {
            return File.Open(FilePath, FileMode.OpenOrCreate);
        }

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public ModuleIdentity ModuleIdentity => ModuleIdentity.Parse(this.ExtractMetaFile().Identity);

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public ModuleIdentity[] Dependencies => this.ExtractMetaFile().Dependencies.Select(ModuleIdentity.Parse).ToArray();
    }
}
