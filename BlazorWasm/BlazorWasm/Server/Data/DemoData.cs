using BlazorWasm.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BlazorWasm.Server.Data
{
    public static class DemoData
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using MandoContext context = new MandoContext(serviceProvider.GetRequiredService<DbContextOptions<MandoContext>>());

            if (context.MandoCharacters.Any())
            {
                return;
            }

            context.MandoCharacters.AddRange(MandoCharacters);
            context.SaveChanges();
        }


        private static MandoCharacter[] MandoCharacters
                => new[]
                {
                    new MandoCharacter
                    {
                        ID = Guid.NewGuid(),
                        Name = "The Mandelorian",
                        Performer = "Pedro Pascal",
                        InSeason1 = true,
                        InSeason2 = true,
                    },
                    new MandoCharacter
                    {
                        ID = Guid.NewGuid(),
                        Name = "Greef Karga",
                        Performer = "Carl Weathers",
                        InSeason1 = true,
                        InSeason2 = true,
                    },
                    new MandoCharacter
                    {
                        ID = Guid.NewGuid(),
                        Name = "Cara Dune",
                        Performer = "Gina Carano",
                        InSeason1 = true,
                        InSeason2 = true,
                    },
                    new MandoCharacter
                    {
                        ID = Guid.NewGuid(),
                        Name = "Moff Gideon",
                        Performer = "Giancarlo Esposito",
                        InSeason1 = true,
                        InSeason2 = true,
                    },
                    new MandoCharacter
                    {
                        ID = Guid.NewGuid(),
                        Name = "Boba Fett",
                        Performer = "Temuera Morrison",
                        InSeason1 = false,
                        InSeason2 = true,
                    }
                };
    }

}