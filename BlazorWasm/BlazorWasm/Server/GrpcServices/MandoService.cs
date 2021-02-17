using BlazorWasm.Server.Data;
using BlazorWasm.Server.Mapper;
using BlazorWasm.Shared.DTOContracts;
using BlazorWasm.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasm.Server.GrpcServices
{
    public class MandoService : IMandoService
    {
        private readonly MandoContext _mandoContext;

        public MandoService(MandoContext mandoContext)
        {
            _mandoContext = mandoContext;
        }

        public async Task<MandoCharacterDetail> AddNewMandoCharacter(MandoCharacterDetail character)
        {
            var mandoCharacter = character.ToEntity();
            _mandoContext.MandoCharacters.Add(mandoCharacter);
            await _mandoContext.SaveChangesAsync();

            return mandoCharacter.ToMandoCharacterDetail();
        }

        public async Task<MandoCharacterDetail> GetMandoCharacterDetails(MandoCharacterDetailRequest request)
        {
            var mandoCharacterDetail = await _mandoContext.MandoCharacters.FindAsync(request.Id);

            if (mandoCharacterDetail == null)
                return null;

            return mandoCharacterDetail.ToMandoCharacterDetail();
        }

        public async Task<IEnumerable<MandoCharacterOverview>> ListMandoCharacters()
        {
            var mandoCharacters = await _mandoContext.MandoCharacters.ToListAsync();

            return mandoCharacters.ToMandoCharacterOverview();
        }
    }
}