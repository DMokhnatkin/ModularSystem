using System.IO;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.PackedModules
{
    public interface IPackedModule : IModule
    {
        Stream OpenStream();

        /// <summary>
        /// Open meta files from packed module. 
        /// </summary>
        MetaFileWrapper ExtractMetaFile();

        /// <summary>
        /// Update meta file in packed module.
        /// </summary>
        void UpdateMetaFile(MetaFileWrapper metaFile);
    }
}
