using System.Runtime.Serialization;

namespace ModularSystem.Communication.Data.Dto
{
    [DataContract]
    public struct ModuleIdentityDto
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public byte ModuleType { get; set; }
    }
}
