using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ModularSystem.Common;
using ModularSystem.Common.Modules;

namespace ModularSystem.Communication.Data.Files
{
    public class ModulesPackage
    {
        public ModulesPackage(IEnumerable<IPathModule> modules)
        {
            PackagedModules = modules.ToArray();
        }

        public IPathModule[] PackagedModules { get; private set; }

        public static async Task<ModulesPackage> Decompress(Stream compressedPackage)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            using (ZipArchive modulePackage = new ZipArchive(compressedPackage))
            {
                modulePackage.ExtractToDirectory(tempPath);
            }

            var modules = new List<IPathModule>();
            foreach (var t in Directory.GetFiles(tempPath))
            {
                var z = new ZipPackagedModule();
                z.InitializeFromPath(t);
                modules.Add(z);
            }

            return new ModulesPackage(modules.ToArray());
        }

        public async Task<Stream> Compress()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            foreach (var module in PackagedModules)
            {
                var p = Path.Combine(path, module.ModuleInfo.ModuleIdentity.ToString());
                File.Copy(module.Path, p);
            }

            string path2 = $"{path}.zip";
            ZipFile.CreateFromDirectory(path, path2);

            return File.Open(path2, FileMode.Open);
        }
    }
}
