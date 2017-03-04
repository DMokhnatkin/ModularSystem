using System;

namespace ModularSystem.Common
{
    /// <summary>
    /// Unique identifier for module.
    /// </summary>
    public struct ModuleIdentity
    {
        /// <summary>
        /// Name of module.
        /// </summary>
        /// <remarks>You can't use spaces in name.</remarks>
        public string Name { get; }

        /// <summary>
        /// Version of module.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// Type of module. F.e. server or client.
        /// </summary>
        /// <see cref="ModuleType"/>
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

        /// <summary>
        /// Parse identity from string.
        /// </summary>
        public static ModuleIdentity Parse(string str)
        {
            var r = str.Split('-');
            return new ModuleIdentity(r[0], (ModuleType)Enum.Parse(typeof(ModuleType), r[1], true), r[2]);
        }
    }
}
