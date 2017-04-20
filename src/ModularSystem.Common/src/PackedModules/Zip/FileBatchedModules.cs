using System.IO;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class FileBatchedModules : ZipBatchedModules
    {
        public string FilePath { get; }

        public FileBatchedModules(string filePath)
        {
            FilePath = filePath;
        }

        /// <inheritdoc />
        public override Stream OpenWriteStream()
        {
            return File.OpenWrite(FilePath);
        }

        /// <inheritdoc />
        public override Stream OpenReadStream()
        {
            return File.OpenRead(FilePath);
        }

        /// <inheritdoc />
        public override Stream OpenEditStream()
        {
            return File.Open(FilePath, FileMode.OpenOrCreate);
        }
    }
}
