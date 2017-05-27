using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;
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

        private MemoryModulesRepository<WpfClientInstalledStoredModuleInfo> _repository = new MemoryModulesRepository<WpfClientInstalledStoredModuleInfo>();

        public void InstallZipPackagedModule(ZipPackedModuleInfo moduleInfo)
        {
            var moduleDir = Path.Combine(BasePath, moduleInfo.ModuleIdentity.ToString());
            if (Directory.Exists(moduleDir))
                Directory.Delete(moduleDir, true);
            Directory.CreateDirectory(moduleDir);

            moduleInfo.UnpackToDirectory(moduleDir);

            // TODO: do smth with dependncy assemblies
            var workingDir = Path.Combine(AppContext.BaseDirectory, "working");
            if (Directory.Exists(workingDir))
                Directory.Delete(workingDir, true);
            Directory.CreateDirectory(workingDir);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(moduleDir, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(moduleDir, workingDir));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(moduleDir, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(moduleDir, workingDir), true);

            var m = new WpfClientInstalledStoredModuleInfo();
            m.InitializeFromPath(moduleDir);
            _repository.AddModule(m);
        }

        public void StartModules()
        {
            var modules = _repository.ToList();
            foreach (var orderModule in ModulesHelper.OrderModules(modules).OfType<WpfClientInstalledStoredModuleInfo>())
            {
                orderModule.Start();
            }
        }
    }
}
