using BlazorWasm.Shared.DTOContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasm.Client.Services
{
    public interface IMandoServiceClient
    {
        event EventHandler ConferenceListChanged;

        Task<MandoCharacterDetail> AddNewMandoCharacter(MandoCharacterDetail character);

        Task<MandoCharacterDetail> GetMandoCharacterDetails(Guid id);

        Task InitAsync();

        Task<List<MandoCharacterOverview>> ListMandoCharacters();
    }
}