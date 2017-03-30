using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
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

        private MemoryModulesRepository<WpfClientInstalledStoredModule> _repository = new MemoryModulesRepository<WpfClientInstalledStoredModule>();

        public void InstallZipPackagedModule(ZipPackagedModule module)
        {
            var moduleDir = Path.Combine(BasePath, module.ModuleIdentity.ToString());
            if (Directory.Exists(moduleDir))
                Directory.Delete(moduleDir, true);
            Directory.CreateDirectory(moduleDir);

            module.UnpackModule(moduleDir);

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

            var m = new WpfClientInstalledStoredModule();
            m.InitializeFromPath(moduleDir);
            _repository.AddModule(m);
        }

        public void StartModules()
        {
            var modules = _repository.ToList();
            foreach (var orderModule in ModulesHelper.OrderModules(modules).OfType<WpfClientInstalledStoredModule>())
            {
                orderModule.Start();
            }
        }
    }
}
