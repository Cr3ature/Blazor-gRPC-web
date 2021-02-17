using System;
using System.Runtime.Serialization;

namespace BlazorWasm.Shared.Contracts
{
    [DataContract]
    public class MandoCharacterOverview
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }
    }
}