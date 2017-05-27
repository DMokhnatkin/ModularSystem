using System;

namespace ModularSystem.Common.Modules
{
    /// <summary>
    /// Unique identifier for module.
    /// </summary>
    public class ModuleIdentity : IModuleIdentity
    {
        /// <inheritdoc />
        public virtual string Name { get; }

        /// <inheritdoc />
        public virtual Version Version { get; }

        /// <inheritdoc />
        public virtual ModuleType Type { get; }

        public ModuleIdentity(string name, ModuleType type, Version version)
        {
            if (name.Contains("-"))
                throw new ArgumentException("Module identity name can't contain -");
            Name = name;
            Type = type;
            Version = version;
        }

        public ModuleIdentity(string name, string type, string version)
        {
            if (name.Contains("-"))
                throw new ArgumentException("Module identity name can't contain -");
            Name = name;
            Type = (ModuleType) Enum.Parse(typeof(ModuleType), type, true);
            Version = new Version(version);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}-{Version}-{Type}";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Version.GetHashCode() ^ Version.GetHashCode();
        }

        /// <summary>
        /// Parse identity from string.
        /// </summary>
        public static ModuleIdentity Parse(string str)
        {
            var r = str.Split('-');
            return new ModuleIdentity(r[0], r[1], r[2]);
        }
    }
}
