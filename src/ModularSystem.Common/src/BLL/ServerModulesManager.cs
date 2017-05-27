using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules.Server;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.BLL
{
    public class ServerModulesManager
    {
        public string ServerModulesStorePath { get; }

        // Cache over repository
        private readonly Dictionary<ModuleIdentity, ServerModule> _serverModules = new Dictionary<ModuleIdentity, ServerModule>();

        public ServerModulesManager(string serverModulesStorePath)
        {
            ServerModulesStorePath = serverModulesStorePath;

            FileSystemHelpers.ClearOrCreateDir(ServerModulesStorePath);
        }

        private ServerModule CreateFromMeta(MetaFileWrapper meta, string path)
        {
            return new ServerModule(
                ModuleIdentity.Parse(meta.Identity),
                meta.Dependencies.Select(ModuleIdentity.Parse).ToArray(),
                path);
        }

        public ServerModule InstallModule(ZipPackedModule module)
        {
            var moduleMeta = module.ExtractMetaFile();

            var modulePath = Path.Combine(ServerModulesStorePath, $"{moduleMeta.Identity}");
            module.UnpackToDirectory(modulePath);

            var newModule = CreateFromMeta(moduleMeta, modulePath);
            _serverModules.Add(newModule.ModuleIdentity, newModule);
            return newModule;
        }

        public ModuleIdentity[] GetInstalledModules()
        {
            return _serverModules.Values.Select(x => x.ModuleIdentity).ToArray();
        }
    }
}
