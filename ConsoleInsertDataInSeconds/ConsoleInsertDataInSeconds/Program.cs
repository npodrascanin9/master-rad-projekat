using ConsoleInsertDataInSeconds;

using MongoDB.Driver;

var mongoKlijent = new MongoClient(@"mongodb://localhost/?replSet=mojaReplika");
var baza = mongoKlijent.GetDatabase("MojaBaza");
var buducePozicijeKolekcija = baza.GetCollection<Pozicija>("buducePozicije");
var pozicijeKolekcija = baza.GetCollection<Pozicija>("pozicije");

// Prvo se brisu dokumenti prilikom pokretanja projekta
await pozicijeKolekcija.DeleteManyAsync(@"{}");

var dokumenti = await buducePozicijeKolekcija.FindAsync(
    x => true);
var pozicijeVozila = await dokumenti.ToListAsync();

var zadaci = new List<Task>
{
    UnesiPozicije(
        pozicije: pozicijeVozila.Where(x => x.VoziloId == 1).ToList(), 
        rnd: new Random()),
    UnesiPozicije(
        pozicije: pozicijeVozila.Where(x => x.VoziloId == 2).ToList(),
        rnd: new Random()),
    UnesiPozicije(
        pozicije: pozicijeVozila.Where(x => x.VoziloId == 3).ToList(), 
        rnd: new Random())
};

await Task.WhenAll(zadaci);

async Task UnesiPozicije(
    List<Pozicija> pozicije, 
    Random rnd)
{
    foreach (var pozicija in pozicije)
    {
        await pozicijeKolekcija.InsertOneAsync(pozicija);
        Console.WriteLine($"Upisana pozicija za vozilo sa Id='{pozicija.VoziloId}'");
        await Task.Delay(rnd.Next(1000, 5000));
    }
}

Console.ReadKey();
