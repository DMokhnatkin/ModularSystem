using System.IO;
using System.Linq;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class MemoryPackedModule : ZipPackedModule
    {
        private readonly byte[] _data;

        public MemoryPackedModule(byte[] data)
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

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public override ModuleIdentity ModuleIdentity => ModuleIdentity.Parse(this.ExtractMetaFile().Identity);

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public override ModuleIdentity[] Dependencies => this.ExtractMetaFile().ClientDependencies.Select(ModuleIdentity.Parse).ToArray();
    }
}
