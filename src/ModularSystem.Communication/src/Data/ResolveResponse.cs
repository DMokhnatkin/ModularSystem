using System.Runtime.Serialization;

namespace ModularSystem.Communication.Data
{
    [DataContract]
    public struct ResolveResponse
    {
        [DataMember]
        public ModuleDto[] Modules { get; set; }
    }
}
