using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularSystem.Common.Exceptions
{
    public class WrongModuleTypeException : ArgumentException
    {
        public ModuleIdentity Module { get; }

        public IEnumerable<ModuleType> AllowedTypes { get; }

        public WrongModuleTypeException(ModuleIdentity module, IEnumerable<ModuleType> allowedTypes = null)
        {
            Module = module;
            AllowedTypes = allowedTypes;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string res = $"Module can't be of type '{Module.ModuleType}'.";
            if (AllowedTypes != null)
                res += $" Allowed types are {string.Concat(AllowedTypes.Select(x => x.ToString() + " "))}";
            return res;
        }
    }
}
