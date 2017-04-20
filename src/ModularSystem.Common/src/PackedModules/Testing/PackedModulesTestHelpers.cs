using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.PackedModules.Testing
{
    /// <summary>
    /// Just some code to help testing.
    /// </summary>
    public class PackedModulesTestHelpers
    {
        public static MemoryPackedModule CreateMemoryPackedModule(string type, ModuleIdentity identity, ModuleIdentity[] dependencies, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            using (var ms = new MemoryStream())
            using (var z = new ZipArchive(ms, ZipArchiveMode.Create))
            {
                var meta = new MetaFileWrapper
                {
                    Type = type,
                    Identity = identity.ToString(),
                    Dependencies = dependencies.Select(x => x.ToString()).ToArray()
                };
                var entry = z.CreateEntry(metaFileName);
                using (var entryStream = entry.Open())
                {
                    meta.Write(entryStream);
                }
                return new MemoryPackedModule(ms.ToArray());
            }
        }
    }
}
