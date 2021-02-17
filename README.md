# Blazor-gRPC-web
Poc of a Blazor WASM using a .Net server host and using gRPC-web code first approach 


# Taken steps

## Step 1

Created a new Blazor WebAssembly project with .NET Hosted server.

````c#
dotnet new blazorwasm --hosted -o your-project-name
````

## Step 2

Add required packages to the shared class library.

````c#
Install-Package protobuf-net.Grpc -Version 1.0.147
Install-Package System.ComponentModel.Annotations -Version 5.0.0
Install-Package System.ServiceModel.Primitives -Version 4.8.1
````

## Step 3

As a Star Wars fan, I allways encapsulate something of that sphere. This time we're going with Characters of the Mandalorian.

We create 2 entity contracts inside the Shared class library.
##### MandoCharacterDetail
````c#
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
````
##### MandoCharacterOverview
````c#
using System;
using System.Runtime.Serialization;

namespace BlazorWasm.Shared.Contracts
{
    [DataContract]
    public class MandoCharacterOverview
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }
    }
}
````

## Step 4
We create a gRPC service contract interface inside the Shared class library
````c#
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

````