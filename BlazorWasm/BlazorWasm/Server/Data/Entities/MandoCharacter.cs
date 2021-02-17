using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWasm.Server.Data.Entities
{
    public class MandoCharacter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ID { get; set; }
        public bool InSeason1 { get; set; }
        public bool InSeason2 { get; set; }
        public string Name { get; set; }
        public string Performer { get; set; }
    }
}