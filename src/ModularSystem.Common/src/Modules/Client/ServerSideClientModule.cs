using ModularSystem.Common.PackedModules;

namespace ModularSystem.Common.Modules.Client
{
    /// <summary>
    /// Client module stored on server (it is just for return it to users)
    /// </summary>
    public class ServerSideClientModule : IClientModule
    {
        public ModuleIdentity ModuleIdentity { get; }
        public ModuleIdentity[] Dependencies { get; }
        public ModuleType ModuleType => ModuleType.Client;
        public string[] ClientTypes { get; }

        public IPackedModule Packed { get; }

        public ServerSideClientModule(ModuleIdentity moduleIdentity, ModuleIdentity[] dependencies, string[] clientTypes, IPackedModule packed)
        {
            ModuleIdentity = moduleIdentity;
            Dependencies = dependencies;
            ClientTypes = clientTypes;
            Packed = packed;
        }
    }
}
