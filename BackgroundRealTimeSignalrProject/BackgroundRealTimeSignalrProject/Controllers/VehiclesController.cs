using BackgroundRealTimeSignalrProject.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BackgroundRealTimeSignalrProject.Controllers
{
    [Route("api/vozila")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        [HttpGet("poslednjePozicije")]
        public async Task<IActionResult> GetWithLastPositions()
        {
            var mongoClient = new MongoClient(@"mongodb://localhost/?replSet=mojaReplika");
            var database = mongoClient.GetDatabase("MojaBaza");
            var kolekcija = database.GetCollection<PoslednjePozicijeVozila_Pregled>(
                name: "vw_poslednjePozicijeVozila");
            var dokumenti = await kolekcija.FindAsync(x => true);
            IEnumerable<PoslednjePozicijeVozila_Pregled> lista = await dokumenti.ToListAsync();
            return Ok(lista);
        }
    }
}
