using BlazorWasm.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasm.Server.Data
{
    public class MandoContext : DbContext
    {
        public MandoContext(DbContextOptions<MandoContext> options)
            : base(options)
        {
        }

        public DbSet<MandoCharacter> MandoCharacters { get; set; }
    }
}