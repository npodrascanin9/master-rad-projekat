using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ConsoleInsertDataInSeconds
{
    public class Pozicija
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("datum")]
        public DateTime DatumUnosa { get; set; }
        [BsonElement("brzina")]
        public double Brzina { get; set; }
        [BsonElement("koordinate")]
        public double[] Koordinate { get; set; }
        [BsonElement("voziloId")]
        public int VoziloId { get; set; }
    }
}
