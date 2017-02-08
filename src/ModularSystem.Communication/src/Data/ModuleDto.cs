using System;
using System.IO;
using System.Runtime.Serialization;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    [DataContract]
    public class ModuleDto : IModule
    {
        /// <inheritdoc />
        [DataMember]
        public ModuleInfo ModuleInfo { get; set; }

        public Stream Data { get; set; }
    }
}
