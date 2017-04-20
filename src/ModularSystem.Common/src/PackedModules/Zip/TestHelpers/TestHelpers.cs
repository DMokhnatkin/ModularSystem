using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.MetaFiles;

namespace ModularSystem.Common.PackedModules.Zip
{
    /// <summary>
    /// Just some code to help testing.
    /// </summary>
    public class TestHelpers
    {
        public static MemoryPackedModule CreateMemoryPackedModule(string type, ModuleIdentity identity, ModuleIdentity[] dependencies, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            using (var ms = new MemoryStream())
            {
                using (var z = new ZipArchive(ms, ZipArchiveMode.Create, true))
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
                }
                return new MemoryPackedModule(ms.ToArray());
            }
        }
    }
}
