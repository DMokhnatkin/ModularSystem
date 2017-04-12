using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ModularSystem.Common.PackedModules.Zip
{
    public static class BatchHelper
    {
        /// <summary>
        /// Pack (batch) list of modules in one zip archive.
        /// </summary>
        /// <param name="destPath">Path of destination zip archive</param>
        /// <param name="packedModules">List of modules to pack</param>
        /// <param name="batch">Result</param>
        public static void BatchModules(IEnumerable<IPackedModuleV2> packedModules, string destPath, out FileBatchedModulesV2 batch)
        {
            using (var fs = File.OpenWrite(destPath))
            using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                foreach (var zipPackedModule in packedModules)
                {
                    var entry = zip.CreateEntry(Guid.NewGuid().ToString());
                    using (var entryStream = entry.Open())
                    using (var moduleStream = zipPackedModule.OpenReadStream())
                    {
                        moduleStream.CopyTo(entryStream);
                    }
                }
            }

            batch = new FileBatchedModulesV2(destPath);
        }

        /// <summary>
        /// Pack (batch) list of modules.
        /// </summary>
        public static void BatchModules(IEnumerable<IPackedModuleV2> packedModules, out MemoryBatchedModulesV2 batch)
        {
            using (var fs = new MemoryStream())
            using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                foreach (var zipPackedModule in packedModules)
                {
                    var entry = zip.CreateEntry(Guid.NewGuid().ToString());
                    using (var entryStream = entry.Open())
                    using (var moduleStream = zipPackedModule.OpenReadStream())
                    {
                        moduleStream.CopyTo(entryStream);
                    }
                }
                batch = new MemoryBatchedModulesV2(fs.ToArray());
            }

        }
    }
}
