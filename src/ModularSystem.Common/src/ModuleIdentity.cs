using System;
using System.Runtime.Serialization;

namespace ModularSystem.Common
{
    public struct ModuleIdentity
    {
        public string Name { get; }

        public Version Version { get; }

        public ModuleType ModuleType { get; }

        public ModuleIdentity(string name, Version version, ModuleType moduleType)
        {
            Name = name;
            Version = version;
            ModuleType = moduleType;
        }

        public ModuleIdentity(string name, string version, ModuleType moduleType)
        {
            Name = name;
            Version = new Version(version);
            ModuleType = moduleType;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} {Version} {ModuleType}";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Version.GetHashCode() ^ ModuleType.GetHashCode();
        }
    }
}
