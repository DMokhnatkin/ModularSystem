using System.IO;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class MemoryBatchedModules : ZipBatchedModules
    {
        private readonly byte[] _data;

        public MemoryBatchedModules(byte[] data)
        {
            _data = data;
        }

        /// <inheritdoc />
        public override Stream OpenWriteStream()
        {
            return new MemoryStream(_data, true);
        }

        /// <inheritdoc />
        public override Stream OpenReadStream()
        {
            return new MemoryStream(_data, false);
        }

        /// <inheritdoc />
        public override Stream OpenEditStream()
        {
            return new MemoryStream(_data, true);
        }
    }
}
