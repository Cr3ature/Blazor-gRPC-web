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

In addition we create a request that we will use for fetching details from the overview list.
##### MandoCharacterDetailRequest
````c#
using System;
using System.Runtime.Serialization;

namespace BlazorWasm.Shared.Contracts
{
    [DataContract]
    public class MandoCharacterDetailRequest
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
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

## Step 5
First lets bootstrap a database so that we can set the entities. We will use a in memory database for POC purpose. This has nothing to do with the gRPC demo but working with data is what we do. So we need data to get a real life scenario.
````c#
// We create an entity with DataAnnotations
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

// Create a db context
    public class MandoContext : DbContext
    {
        public MandoContext(DbContextOptions<MandoContext> options)
            : base(options)
        {
        }

        public DbSet<MandoCharacter> MandoCharacters { get; set; }
    }

// Create a seed class for seeding the in memory database
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

// Bootstrap the dbcontext in the startup 
    public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MandoContext>(
                options => options.UseInMemoryDatabase(databaseName: "BlazorWasm"));

           ...
        }

// Configure the pipeline in the startup to use the Grpc web en register the endpoints.
   public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MandoService>().EnableGrpcWeb();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

// Change the Program startup to seed the webhost
    public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {

            var host = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build())                
                .UseStartup<Startup>()
                .Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<MandoContext>();
            DemoData.Seed(services);

            return host;
        }
````

## Step 6
Almost there, we have created a server with an inmemory database that exposes a gRPC endpoint with it's gRPC datacontract interface inside the shared folder. Instead of creating a proto and expose it through the .csproj. Let's consume the interface directly from the client and bootstrap the remaining loose ends. I kid you not, that's all that remains.
````c#

// Add a service interface
    public interface IMandoServiceClient
    {
        event EventHandler ConferenceListChanged;

        Task<MandoCharacterDetail> AddNewMandoCharacter(MandoCharacterDetail character);

        Task<MandoCharacterDetail> GetMandoCharacterDetails(Guid id);

        Task InitAsync();

        Task<List<MandoCharacterOverview>> ListMandoCharacters();
    }
// Add the implementation
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

// We register the serviceclient and its interface and setup a gRPC channel inside the program.cs
    public static async Task Main(string[] args)
    {
        ...
        builder.Services.AddScoped<GrpcChannel>(services =>
        {
            var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(builder.HostEnvironment.BaseAddress, new GrpcChannelOptions { HttpHandler = grpcWebHandler });

            return channel;
        });

        builder.Services.AddScoped<IMandoServiceClient, MandoServiceGrpcClient>();

        await builder.Build().RunAsync();
    }
````

And we are done
Let's put it in play on the index page

````C#
@page "/"

@using BlazorWasm.Shared.DTOContracts
@using BlazorWasm.Client.Services

@inject IMandoServiceClient _serviceClient

<h1>Hello, world!</h1>

Welcome to your new app.

<p>Below is example of a list overview fetched using a gRPC channel.</p>

@if (_mandoCharacters == null)
{
    <p>Loading...</p>
}
else
{
    <table class="table table-responsive">
        <thead>
            <tr>
                <th scope="col">Id</th>
                <th scope="col">Character Name</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var mandoCharacter in _mandoCharacters)
            {
                <tr>
                    <th scope="row">@mandoCharacter.Id</th>
                    <th>@mandoCharacter.Name</th>
                </tr>
            }
        </tbody>
    </table>
}

@code{
    private IReadOnlyList<MandoCharacterOverview> _mandoCharacters;

    protected override async Task OnInitializedAsync()
    {
        _mandoCharacters = await _serviceClient.ListMandoCharacters();
    }
}

````
