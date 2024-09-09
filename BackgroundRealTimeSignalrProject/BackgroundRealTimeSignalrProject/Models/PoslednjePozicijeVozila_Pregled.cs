using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackgroundRealTimeSignalrProject.Models
{
    public class PoslednjePozicijeVozila_Pregled
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; } = default;
        [BsonElement("tablica")]
        public string Tablica { get; set; }
        [BsonElement("tip")]
        public string Tip { get; set; }
        [BsonElement("poslednjaPozicija")]
        public PoslednjaPozicijaOdgovor PoslednjaPozicija { get; set; }

        [BsonIgnore]
        public string GrafikonId
        {
            get => $"grafikon-{Id}";
        }

        [BsonIgnore]
        public object Grafikon
        {
            get => null;
        }

        [BsonIgnore]
        public IEnumerable<DiagramData> Podaci
        {
            get => new List<DiagramData>()
            {
                new DiagramData()
                {
                    Y = PoslednjaPozicija.BrzinaKretanja,
                    X = PoslednjaPozicija.DatumUnosa.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")
                }
            };
        }
    }

    public class DiagramData
    {
        public string X { get; set; }
        public double Y { get; set; }
    }

    public class PoslednjaPozicijaOdgovor
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("koordinate")]
        public double[]? Koordinate { get; set; }
        [BsonElement("brzina")]
        public double BrzinaKretanja { get; set; }
        [BsonElement("datum")]
        public DateTime DatumUnosa { get; set; }
    }
}
