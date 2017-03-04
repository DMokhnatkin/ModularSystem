using System.IO;
using System.IO.Compression;
using ModularSystem.Common.Modules.Generic;

namespace ModularSystem.Common
{
    public class ZipPackagedModule : IPackagedModule<ZipArchive>
    {
        /// <inheritdoc />
        public ModuleInfo ModuleInfo { get; set; }

        /// <inheritdoc />
        public Stream Data { get; set; }

        /// <inheritdoc />
        ZipArchive IPackagedModule<ZipArchive>.Data => new ZipArchive(Data);
    }
}
