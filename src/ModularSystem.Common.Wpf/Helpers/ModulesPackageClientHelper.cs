using System;
using System.IO;
using System.IO.Compression;
using ModularSystem.Communication.Data.Files;

namespace ModularSystem.Common.Wpf.Helpers
{
    public static class ModulesPackageClientHelper
    {
        public static void InstallToClient(this ModulesPackage package, string basePath)
        {
            foreach (var packageModule in package.Modules)
            {
                var t = Directory.CreateDirectory(Path.Combine(basePath, packageModule.ModuleInfo.ModuleIdentity.ToString()));
                using (ZipArchive z = new ZipArchive(packageModule.Data))
                {
                    z.ExtractToDirectory(t.FullName);
                }
            }
        }
    }
}
