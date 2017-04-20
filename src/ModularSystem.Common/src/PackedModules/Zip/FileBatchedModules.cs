using System.IO;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class FileBatchedModules : IZipBatchedModules
    {
        public string FilePath { get; }

        public FileBatchedModules(string filePath)
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
