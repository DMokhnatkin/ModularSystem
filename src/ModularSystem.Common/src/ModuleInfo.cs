using System;
using System.Runtime.Serialization;

namespace ModularSystem.Common
{
    [DataContract]
    public class ModuleInfo
    {
        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public Version Version { get; private set; }

        [DataMember]
        public ModuleInfo[] Dependencies { get; private set; }

        public ModuleInfo(string name, Version version, ModuleInfo[] dependencies)
        {
            Name = name;
            Version = version;
            Dependencies = dependencies;
        }
    }
}
