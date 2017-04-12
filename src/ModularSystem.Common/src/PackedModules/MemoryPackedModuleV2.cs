using System.IO;

namespace ModularSystem.Common.PackedModules
{
    public class MemoryPackedModuleV2 : IPackedModuleV2
    {
        private readonly byte[] _data;

        public MemoryPackedModuleV2(byte[] data)
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
