using System.Runtime.Serialization;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    [DataContract]
    public class ModuleDto
    {
        /// <inheritdoc />
        [DataMember]
        public ModuleInfo ModuleInfo { get; set; }

        [DataMember]
        public byte[] Data { get; set; }
    }
}
