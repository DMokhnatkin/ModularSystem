using System.Collections.Generic;
using System.Runtime.Serialization;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    [DataContract]
    public class ResolveRequest : IResolveRequest
    {
        /// <inheritdoc />
        public IEnumerable<ModuleIdentity> ModuleIdentities { get; set; }
    }
}
