using System.IO;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class MemoryBatchedModules : IBatchedModules
    {
        public byte[] Data { get; }

        public MemoryBatchedModules(byte[] data)
        {
            Data = data;
        }

        /// <inheritdoc />
        public Stream OpenStream()
        {
            return new MemoryStream(Data);
        }
    }
}
