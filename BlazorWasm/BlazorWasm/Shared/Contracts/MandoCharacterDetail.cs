using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BlazorWasm.Shared.Contracts
{
    [DataContract]
    public class MandoCharacterDetail
    {
        [DataMember(Order = 1)]
        public Guid ID { get; set; }

        [DataMember(Order = 2)]
        [Required]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        [Required]
        public string Performer { get; set; }

        [DataMember(Order = 4)]
        [Required]
        public bool InSeason1 { get; set; }

        [DataMember(Order = 5)]
        [Required]
        public bool InSeason2 { get; set; }
    }
}