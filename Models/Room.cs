using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hotel_Reservation.Models
{
    public class Room
    {
        [BsonId]
        [BsonElement("_id")]
        public string Id { get; set; }  // Change to `int?` (nullable integer)

        [BsonElement("RoomNo")]
        public string RoomNo { get; set; }

        [BsonElement("Floor")]
        public int Floor { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Amenities")]
        public string Amenities { get; set; }
    }
}
