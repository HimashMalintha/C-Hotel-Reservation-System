using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hotel_Reservation.Models
{
    public class Feedback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ReservationId { get; set; }  

        public string CustomerEmail { get; set; }

        public string Message { get; set; }

        public int Rating { get; set; }
    }
}
