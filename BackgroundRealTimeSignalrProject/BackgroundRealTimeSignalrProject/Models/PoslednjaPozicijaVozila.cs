using MongoDB.Bson;
using Newtonsoft.Json;

namespace BackgroundRealTimeSignalrProject.Models
{
    public class PoslednjaPozicijaVozila
    {
        public string Id { get; set; }
        public double VoziloId { get; set; }
        public DateTime DatumUnosa { get; set; }
        public double Brzina { get; set; }
        public double[] Koordinate { get; set; }

        public string DatumUnosaFormat
        {
            get => DatumUnosa.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
        }
    }
}
