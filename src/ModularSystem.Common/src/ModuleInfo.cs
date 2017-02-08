using System.Runtime.Serialization;

namespace ModularSystem.Common
{
    [DataContract]
    public class ModuleInfo
    {
        [DataMember]
        public ModuleIdentity ModuleIdentity { get; set; }

        [DataMember]
        public ModuleInfo[] Dependencies { get; private set; }

        public ModuleInfo(ModuleIdentity identity, ModuleInfo[] dependencies)
        {
            ModuleIdentity = identity;
            Dependencies = dependencies;
        }
    }
}
