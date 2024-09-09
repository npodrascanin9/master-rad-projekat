using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackgroundRealTimeSignalrProject.Models
{
    public class Pozicija
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("koordinate")]
        public double[]? Koordinate { get; set; }
        [BsonElement("brzina")]
        public double BrzinaKretanja { get; set; }
        [BsonElement("datum")]
        public DateTime DatumUnosa { get; set; } = DateTime.Now;
        [BsonElement("voziloId")]
        public int VoziloId { get; set; }
    }
}
