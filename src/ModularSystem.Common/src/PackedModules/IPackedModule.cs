using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.PackedModules
{
    public interface IPackedModule : IPacked, IModule
    {
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
