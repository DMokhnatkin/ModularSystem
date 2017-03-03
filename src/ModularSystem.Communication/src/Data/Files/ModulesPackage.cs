using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Mappers;

namespace ModularSystem.Communication.Data.Files
{
    public class ModulesPackage
    {
        public ModulesPackage(IEnumerable<IModule> modules)
        {
            Modules = modules.ToArray();
        }

        public IModule[] Modules { get; private set; }

        public static async Task<ModulesPackage> Decompress(Stream compressedPackage)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            using (ZipArchive modulePackage = new ZipArchive(compressedPackage))
            {
                modulePackage.ExtractToDirectory(tempPath);
            }

            var modules = new List<IModule>();
            foreach (var t in Directory.GetDirectories(tempPath))
            {
                modules.Add(await ModuleDtoFileSystem.ReadFromDirectory(t).Unwrap());
            }

            return new ModulesPackage(modules.ToArray());
        }

        public async Task<Stream> Compress()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            foreach (var module in Modules)
            {
                var p = Path.Combine(path, module.ModuleInfo.ModuleIdentity.ToString());
                (await module.Wrap()).WriteToDirectory(p);
            }

            string path2 = $"{path}.zip";
            ZipFile.CreateFromDirectory(path, path2);

            return File.Open(path2, FileMode.Open);
        }
    }
}
