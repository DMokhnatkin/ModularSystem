using System.IO;
using System.Linq;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class MemoryPackedModule : IZipPackedModule
    {
        private readonly byte[] _data;

        public MemoryPackedModule(byte[] data)
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

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public ModuleIdentity ModuleIdentity => ModuleIdentity.Parse(this.ExtractMetaFile().Identity);

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public ModuleIdentity[] Dependencies => this.ExtractMetaFile().Dependencies.Select(ModuleIdentity.Parse).ToArray();
    }
}
