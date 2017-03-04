using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ModularSystem.Common.Modules;
using ModularSystem.Communication.Data.Files;

namespace ModularSystem.Common.Wpf.Helpers
{
    public static class ModulesPackageClientHelper
    {
        public static void InstallToClient(this ModulesPackage package, string basePath)
        {
            List<IPathModule> res = new List<IPathModule>();
            foreach (var packageModule in package.PackagedModules)
            {
                var t = Directory.CreateDirectory(Path.Combine(basePath, packageModule.ModuleInfo.ModuleIdentity.ToString()));
                using (ZipArchive z = new ZipArchive(File.OpenRead(packageModule.Path)))
                {
                    z.ExtractToDirectory(t.FullName);
                }
            }
        }
    }
}
