using System.IO;
using System.IO.Compression;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common
{
    public class ZipPackagedModule : IPathModule
    {
        /// <inheritdoc />
        public ModuleInfo ModuleInfo { get; set; }

        /// <inheritdoc />
        public string Path { get; set; }
    }
}
