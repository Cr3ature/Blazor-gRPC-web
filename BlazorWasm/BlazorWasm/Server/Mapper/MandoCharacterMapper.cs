using BlazorWasm.Server.Data.Entities;
using BlazorWasm.Shared.DTOContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorWasm.Server.Mapper
{
    public static class MandoCharacterMapper
    {
        public static MandoCharacter ToEntity(this MandoCharacterDetail mandoCharacterDetail)
            => new MandoCharacter
            {
                ID = mandoCharacterDetail.ID,
                InSeason1 = mandoCharacterDetail.InSeason1,
                InSeason2 = mandoCharacterDetail.InSeason2,
                Name = mandoCharacterDetail.Name,
                Performer = mandoCharacterDetail.Performer,
            };

        public static MandoCharacterDetail ToMandoCharacterDetail(this MandoCharacter mandoCharacter)
            => new MandoCharacterDetail
            {
                ID = mandoCharacter.ID,
                InSeason1 = mandoCharacter.InSeason1,
                InSeason2 = mandoCharacter.InSeason2,
                Name = mandoCharacter.Name,
                Performer = mandoCharacter.Performer,
            };

        public static IEnumerable<MandoCharacterOverview> ToMandoCharacterOverview(this IEnumerable<MandoCharacter> mandoCharacters)
            => mandoCharacters?.Select(s => s.ToMandoCharacterOverviewItem()) ?? Array.Empty<MandoCharacterOverview>();

        private static MandoCharacterOverview ToMandoCharacterOverviewItem(this MandoCharacter mandoCharacter)
            => new MandoCharacterOverview
            {
                Id = mandoCharacter.ID,
                Name = mandoCharacter.Name,
            };
    }
}