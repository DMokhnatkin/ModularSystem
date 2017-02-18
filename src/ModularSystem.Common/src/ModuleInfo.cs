namespace ModularSystem.Common
{
    public class ModuleInfo
    {
        public ModuleIdentity ModuleIdentity { get; }

        public ModuleIdentity[] Dependencies { get; }

        public ModuleInfo(ModuleIdentity identity, ModuleIdentity[] dependencies)
        {
            ModuleIdentity = identity;
            Dependencies = dependencies;
        }
    }
}
