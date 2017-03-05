using System.IO;
using System.IO.Compression;
using ModularSystem.Common.Repositories;

namespace ModularSystem.Common.Wpf.Modules
{
    public class InstalledModuleCollection
    {
        public string BasePath { get; }

        public InstalledModuleCollection(string path)
        {
            BasePath = path;
            if (Directory.Exists(BasePath))
                Directory.Delete(BasePath, true);
        }

        private MemoryModulesRepository<WpfClientInstalledModule> _repository = new MemoryModulesRepository<WpfClientInstalledModule>();

        public void InstallZipPackagedModule(ZipPackagedModule module)
        {
            var t = Path.Combine(BasePath, module.ModuleInfo.ModuleIdentity.ToString());
            if (Directory.Exists(t))
                Directory.Delete(t, true);
            Directory.CreateDirectory(t);

            using (ZipArchive z = new ZipArchive(File.OpenRead(module.Path)))
            {
                z.ExtractToDirectory(t);
            }

            var m = new WpfClientInstalledModule();
            m.InitializeFromPath(t);
            _repository.AddModule(m);
        }
    }
}
