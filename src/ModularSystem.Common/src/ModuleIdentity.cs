using System;
using System.Runtime.Serialization;

namespace ModularSystem.Common
{
    [DataContract]
    public struct ModuleIdentity
    {
        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public Version Version { get; private set; }

        public ModuleIdentity(string name, Version version)
        {
            Name = name;
            Version = version;
        }

        public ModuleIdentity(string name, string version)
        {
            Name = name;
            Version = new Version(version);
        }
    }
}
