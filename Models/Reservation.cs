using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Hotel_Reservation.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  // ✅ MongoDB will generate `_id`
        public string Id { get; set; }

        [BsonElement("ReservationId")]  // ✅ Store Res1, Res2, etc.
        public string ReservationId { get; set; }

        [BsonElement("CustomerEmail")]
        public string CustomerEmail { get; set; }

        [BsonElement("RoomNo")]
        public string RoomNo { get; set; }

        [BsonElement("RoomType")]  // ✅ Now sending Room Type instead of Amenities
        public string RoomType { get; set; }

        [BsonElement("CheckInDate")]
        public DateTime CheckInDate { get; set; }

        [BsonElement("CheckOutDate")]
        public DateTime CheckOutDate { get; set; }

        [BsonElement("TotalPrice")]
        public decimal TotalPrice { get; set; }

        [BsonElement("BookingDate")]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    }
}
