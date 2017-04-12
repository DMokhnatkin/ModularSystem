using System.IO;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class FileBatchedModulesV2 : IBatchedModulesV2
    {
        public string FilePath { get; }

        public FileBatchedModulesV2(string filePath)
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
    }
}
