using System.Runtime.Serialization;

namespace ModularSystem.Communication.Data.Dto
{
    [DataContract]
    public class ModuleDto
    {
        [DataMember]
        public ModuleInfoDto ModuleInfo { get; set; }

        [DataMember]
        public byte[] Data { get; set; }
    }
}
