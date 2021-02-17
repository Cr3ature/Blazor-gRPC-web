using System;
using System.Runtime.Serialization;

namespace BlazorWasm.Shared.DTOContracts
{
    [DataContract]
    public class MandoCharacterDetailRequest
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}