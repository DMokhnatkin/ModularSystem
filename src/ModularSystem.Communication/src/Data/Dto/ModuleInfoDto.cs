using System.Runtime.Serialization;

namespace ModularSystem.Communication.Data.Dto
{
    [DataContract]
    public struct ModuleInfoDto
    {
        [DataMember]
        public ModuleIdentityDto ModuleIdentity { get; set; }

        [DataMember]
        public ModuleIdentityDto[] Dependencies { get; set; }
    }
}
