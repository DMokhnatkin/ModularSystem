using System.IO;

namespace ModularSystem.Common
{
    public class Module : IModule
    {
        /// <inheritdoc />
        public ModuleInfo ModuleInfo { get; set; }

        /// <inheritdoc />
        public Stream Data { get; set; }
    }
}
