using BlazorWasm.Shared.Contracts;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace BlazorWasm.Shared.Services
{
    [ServiceContract]
    public interface IMandoService
    {
        Task<IEnumerable<MandoCharacterOverview>> ListMandoCharacters();
        Task<MandoCharacterDetail> GetMandoCharacterDetails(MandoCharacterDetailRequest request);
        Task<MandoCharacterDetail> AddNewMandoCharacter(MandoCharacterDetail character);
    }
}
