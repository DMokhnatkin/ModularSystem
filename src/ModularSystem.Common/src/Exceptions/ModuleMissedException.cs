using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularSystem.Common.Exceptions
{
    public class ModuleMissedException : ArgumentException
    {
        public IEnumerable<ModuleIdentity> Missed { get; }

        public ModuleMissedException(ModuleIdentity module) : base()
        {
            Missed = new[] { module };
        }

        public ModuleMissedException(IEnumerable<ModuleIdentity> modules) : base()
        {
            Missed = modules;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string missed = string.Concat(Missed.Select(x => x.ToString() + " "));
            return $"One or more modules are missed. {missed}";
        }
    }
}
