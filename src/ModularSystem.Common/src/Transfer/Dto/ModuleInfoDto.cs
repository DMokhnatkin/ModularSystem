using System.Runtime.Serialization;

namespace ModularSystem.Common.Transfer.Dto
{
    [DataContract]
    public struct ModuleInfoDto
    {
        [DataMember]
        public string ModuleIdentity { get; set; }

        [DataMember]
        public string[] Dependencies { get; set; }
    }
}
