using BackgroundRealTimeSignalrProject.Hubs;
using BackgroundRealTimeSignalrProject.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace BackgroundRealTimeSignalrProject.Services
{
    public class VehiclePositionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public VehiclePositionBackgroundService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                await InicijalizujProces();
                // await Task.Delay(5000, stoppingToken); // Delay for 5 seconds
            }
        }

        private async Task InicijalizujProces()
        {
            var mongoClient = new MongoClient(@"mongodb://localhost/?replSet=mojaReplika");
            var database = mongoClient.GetDatabase("MojaBaza");
            var collection = database.GetCollection<BsonDocument>("pozicije");
            var options = new ChangeStreamOptions()
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };

            var opcije = new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };
            var filterKonfiguracija = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
                .Match(@"
                    {
                        'operationType': 'insert'
                    }
                ")
                .Project(@"
                    {
                        'fullDocument': {
                            '_id': true,
                            'voziloId': true,
                            'brzina': true,
                            'datum': true,
                            'koordinate': true
                        }
                    }
                ");

            var tokPromena = await collection.WatchAsync<BsonDocument>(filterKonfiguracija, opcije);
            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<PozicijeVozilaHab>>();

                await tokPromena.ForEachAsync(async dogadjaj =>
                {
                    var bsonDokument = dogadjaj["fullDocument"].ToBsonDocument();
                    // 2024-09-03T18:42:42.809Z
                    var poslednjaPozicijaVozila = new PoslednjaPozicijaVozila()
                    {
                        Id = bsonDokument["_id"].ToString(),
                        Brzina = Convert.ToDouble(bsonDokument["brzina"].ToString()),
                        DatumUnosa = Convert.ToDateTime(bsonDokument["datum"].ToString()),
                        VoziloId = Convert.ToDouble(bsonDokument["voziloId"].ToString()),
                        Koordinate = JsonConvert.DeserializeObject<double[]>(bsonDokument["koordinate"].ToString())
                    };
                    await hubContext.Clients.All.SendAsync(
                        method: "mojametoda", 
                        arg1: poslednjaPozicijaVozila);
                });
            }
        }
    }
}
