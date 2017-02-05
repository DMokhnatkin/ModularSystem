using System.Runtime.Serialization;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    [DataContract]
    public class ModuleDto
    {
        [DataMember]
        public ModuleIdentity ModuleIdentity { get; set; }

        [DataMember]
        public string[] Dependencies { get; set; }

        [DataMember]
        public byte[] Data { get; set; }
    }
}
