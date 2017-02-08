using System.Runtime.Serialization;

namespace ModularSystem.Common
{
    [DataContract]
    public enum ModuleType
    {
        [EnumMember]
        Client,
        [EnumMember]
        Server
    }
}