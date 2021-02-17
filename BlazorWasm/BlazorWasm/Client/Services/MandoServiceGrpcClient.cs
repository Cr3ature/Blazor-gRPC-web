using BlazorWasm.Shared.DTOContracts;
using BlazorWasm.Shared.Services;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasm.Client.Services
{
    public class MandoServiceGrpcClient : IMandoServiceClient
    {
        private readonly IMandoService _mandoService;

        public MandoServiceGrpcClient(GrpcChannel channel)
        {
            _mandoService = channel.CreateGrpcService<IMandoService>();
        }

        public event EventHandler ConferenceListChanged;

        public async Task<MandoCharacterDetail> AddNewMandoCharacter(MandoCharacterDetail character)
            => await _mandoService.AddNewMandoCharacter(character);

        public async Task<MandoCharacterDetail> GetMandoCharacterDetails(Guid id)
            => await _mandoService.GetMandoCharacterDetails(new MandoCharacterDetailRequest { Id = id });

        public Task InitAsync()
            => Task.CompletedTask;

        public async Task<List<MandoCharacterOverview>> ListMandoCharacters()
            => (await _mandoService.ListMandoCharacters()).ToList();
    }
}