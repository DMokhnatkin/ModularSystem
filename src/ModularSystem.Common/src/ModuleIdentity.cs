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

        public ModuleIdentity(string name, Version version)
        {
            if (name.Contains("-"))
                throw new ArgumentException("Module identity name can't contain -");
            Name = name;
            Version = version;
        }

        public ModuleIdentity(string name, string version)
        {
            if (name.Contains("-"))
                throw new ArgumentException("Module identity name can't contain -");
            Name = name;
            Version = new Version(version);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}-{Version}";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Version.GetHashCode();
        }

        /// <summary>
        /// Parse identity from string.
        /// </summary>
        public static ModuleIdentity Parse(string str)
        {
            var r = str.Split('-');
            return new ModuleIdentity(r[0], r[2]);
        }
    }
}
