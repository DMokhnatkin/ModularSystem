using System.Collections.Generic;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    public interface IResolveResponse
    {
        IEnumerable<ModuleIdentity> ModuleIdentities { get; }
    }
}