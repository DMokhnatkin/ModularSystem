using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModularSystem.Common.Modules.Client;
using ModularSystem.Common.Modules.Server;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.BLL
{
    public class ServerModulesManager
    {
        public string ServerModulesStorePath { get; }

        private readonly Dictionary<ModuleIdentity, ServerModule> _serverModules = new Dictionary<ModuleIdentity, ServerModule>();

        public ServerModulesManager(string serverModulesStorePath)
        {
            ServerModulesStorePath = serverModulesStorePath;

            FileSystemHelpers.ClearOrCreateDir(ServerModulesStorePath);
        }

        public ServerModule InstallModule(ZipPackedModule module)
        {
            var moduleMeta = module.ExtractMetaFile();

            var modulePath = Path.Combine(ServerModulesStorePath, $"{moduleMeta.Identity}");
            module.UnpackToDirectory(modulePath);

            var newModule = new ServerModule(
                ModuleIdentity.Parse(moduleMeta.Identity), 
                moduleMeta.Dependencies.Select(ModuleIdentity.Parse).ToArray(),
                modulePath);
            _serverModules.Add(newModule.ModuleIdentity, newModule);
            return newModule;
        }

        public ModuleIdentity[] GetInstalledModules()
        {
            return _serverModules.Values.Select(x => x.ModuleIdentity).ToArray();
        }
    }
}
