using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Common.Wpf.Modules
{
    public class InstalledModulesRepository : IModulesRepository<InstalledModule>
    {
        public static void InstallZipPackagedModule(ZipPackagedModule module, string path)
        {
            using (ZipArchive z = new ZipArchive(File.OpenRead(module.Path)))
            {
                var t = Path.Combine(path, module.ModuleInfo.ToString());
                if (Directory.Exists(t))
                    Directory.Delete(t, true);
                Directory.CreateDirectory(t);
                z.ExtractToDirectory(t);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<InstalledModule> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void AddModule(InstalledModule module)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public InstalledModule GetModule(ModuleIdentity moduleIdentity)
        {
            throw new System.NotImplementedException();
        }
    }
}
