using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hotel_Reservation.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; } // CS001, CS002 format

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("contact")]
        public string Contact { get; set; } // ✅ Fixes 'User' does not contain 'Contact' Error

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
