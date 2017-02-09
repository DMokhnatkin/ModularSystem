using System.Runtime.Serialization;

namespace ModularSystem.Common
{
    [DataContract]
    public class ModuleInfo
    {
        [DataMember]
        public ModuleIdentity ModuleIdentity { get; set; }

        [DataMember]
        public ModuleIdentity[] Dependencies { get; private set; }

        public ModuleInfo(ModuleIdentity identity, ModuleIdentity[] dependencies)
        {
            ModuleIdentity = identity;
            Dependencies = dependencies;
        }
    }
}
