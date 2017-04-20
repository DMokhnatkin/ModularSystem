using System.IO;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class MemoryBatchedModules : IZipBatchedModules
    {
        private readonly byte[] _data;

        public MemoryBatchedModules(byte[] data)
        {
            _data = data;
        }

        /// <inheritdoc />
        public Stream OpenWriteStream()
        {
            return new MemoryStream(_data, true);
        }

        /// <inheritdoc />
        public Stream OpenReadStream()
        {
            return new MemoryStream(_data, false);
        }

        /// <inheritdoc />
        public Stream OpenEditStream()
        {
            return new MemoryStream(_data, true);
        }
    }
}
