using System;

namespace ModularSystem.Common
{
    public struct ModuleIdentity
    {
        public string Name { get; }

        public Version Version { get; }

        public ModuleType ModuleType { get; }

        public ModuleIdentity(string name, ModuleType moduleType, Version version)
        {
            if (name.Contains(" "))
                throw new ArgumentException("Module identity name can't contain spaces");
            Name = name;
            Version = version;
            ModuleType = moduleType;
        }

        public ModuleIdentity(string name, ModuleType moduleType, string version)
        {
            if (name.Contains(" "))
                throw new ArgumentException("Module identity name can't contain spaces");
            Name = name;
            Version = new Version(version);
            ModuleType = moduleType;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}-{ModuleType}-{Version}";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Version.GetHashCode() ^ ModuleType.GetHashCode();
        }

        public static ModuleIdentity Parse(string str)
        {
            var r = str.Split('-');
            return new ModuleIdentity(r[0], (ModuleType)Enum.Parse(typeof(ModuleType), r[1]), r[2]);
        }
    }
}
