using MongoDB.Driver;
using Hotel_Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotel_Reservation
{
    public class DatabaseHelper
    {
        private readonly IMongoCollection<Room> _roomCollection;
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Feedback> _feedbackCollection;

        public DatabaseHelper()
        {
            try
            {
                string connectionString = "mongodb+srv://abc:1234@cluster0.3riui.mongodb.net/?retryWrites=true&w=majority";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("HotelDB");

                _roomCollection = database.GetCollection<Room>("Rooms");
                _userCollection = database.GetCollection<User>("Users");
                _reservationCollection = database.GetCollection<Reservation>("Reservations");
                _feedbackCollection = database.GetCollection<Feedback>("Feedbacks");

                Console.WriteLine("✅ MongoDB Connection Established Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error Connecting to MongoDB: {ex.Message}");
            }
        }

        // ✅ **GET LAST RESERVATION**
        public Reservation GetLastReservation()
        {
            return _reservationCollection.Find(_ => true)
                                         .SortByDescending(r => r.ReservationId)
                                         .FirstOrDefault();
        }

        // ✅ **CHECK IF EMAIL EXISTS**
        public bool CheckUserExists(string email)
        {
            return _userCollection.Find(u => u.Email == email).Any();
        }

        // ✅ **REGISTER USER**
        public void RegisterUser(User user)
        {
            try
            {
                if (CheckUserExists(user.Email))
                {
                    Console.WriteLine("❌ Email already exists. Skipping registration.");
                    return;
                }

                var lastUser = _userCollection.Find(_ => true)
                                              .SortByDescending(u => u.UserId)
                                              .FirstOrDefault();

                int newId = (lastUser != null && int.TryParse(lastUser.UserId.Substring(2), out int lastId)) ? lastId + 1 : 1;
                user.UserId = $"CS{newId:D3}";

                _userCollection.InsertOne(user);
                Console.WriteLine($"✅ User Registered: {user.UserId}, {user.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error registering user: {ex.Message}");
            }
        }

        // ✅ **AUTHENTICATE USER**
        public User AuthenticateUser(string email, string password)
        {
            return _userCollection.Find(u => u.Email == email && u.Password == password).FirstOrDefault();
        }

        // ✅ **UPDATE USER PROFILE**
        public bool UpdateUser(User updatedUser)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Email, updatedUser.Email);
                var update = Builders<User>.Update
                    .Set(u => u.Name, updatedUser.Name)
                    .Set(u => u.Address, updatedUser.Address)
                    .Set(u => u.Contact, updatedUser.Contact)
                    .Set(u => u.Password, updatedUser.Password);

                var result = _userCollection.UpdateOne(filter, update);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error updating user: {ex.Message}");
                return false;
            }
        }

        // ✅ **ROOM MANAGEMENT**
        public void AddRoom(Room room)
        {
            try
            {
                if (DoesRoomExist(room.RoomNo))
                {
                    Console.WriteLine("❌ Duplicate Room Detected! Skipping insertion.");
                    return;
                }

                string lastId = GetLastRoomId();
                int newId = int.TryParse(lastId.Substring(2), out int lastNumericId) ? lastNumericId + 1 : 1;
                room.Id = $"RM{newId:D3}";

                _roomCollection.InsertOne(room);
                Console.WriteLine($"✅ Room Added with _id: {room.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error adding room: {ex.Message}");
            }
        }

        // ✅ **GET ALL ROOMS**
        public List<Room> GetRooms()
        {
            return _roomCollection.Find(_ => true).ToList();
        }

        // ✅ **GET LAST ROOM ID**
        public string GetLastRoomId()
        {
            var lastRoom = _roomCollection.Find(_ => true)
                                          .SortByDescending(r => r.Id)
                                          .FirstOrDefault();

            return lastRoom?.Id ?? "RM000";  // If no rooms exist, return RM000
        }

        public void DeleteRoom(string roomNo) => _roomCollection.DeleteOne(r => r.RoomNo == roomNo);
        public bool DoesRoomExist(string roomNo) => _roomCollection.Find(r => r.RoomNo == roomNo).Any();

        // ✅ **UPDATE ROOM**
        public bool UpdateRoom(string roomNo, Room updatedRoom)
        {
            try
            {
                var filter = Builders<Room>.Filter.Eq(r => r.RoomNo, roomNo);
                var update = Builders<Room>.Update
                    .Set(r => r.Floor, updatedRoom.Floor)
                    .Set(r => r.Price, updatedRoom.Price)
                    .Set(r => r.Type, updatedRoom.Type)
                    .Set(r => r.Amenities, updatedRoom.Amenities);

                var result = _roomCollection.UpdateOne(filter, update);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error updating room: {ex.Message}");
                return false;
            }
        }

        // ✅ **RESERVATION MANAGEMENT**
        public void AddReservation(Reservation reservation)
        {
            try
            {
                var lastReservation = GetLastReservation();
                int newId = 1;

                if (lastReservation != null && lastReservation.ReservationId.StartsWith("Res"))
                {
                    string lastIdStr = lastReservation.ReservationId.Replace("Res", "");
                    if (int.TryParse(lastIdStr, out int lastNumericId))
                    {
                        newId = lastNumericId + 1;
                    }
                }

                reservation.ReservationId = $"Res{newId}";
                reservation.CheckInDate = reservation.CheckInDate.Date;
                reservation.CheckOutDate = reservation.CheckOutDate.Date;
                reservation.BookingDate = DateTime.UtcNow;

                _reservationCollection.InsertOne(reservation);
                Console.WriteLine($"✅ Reservation Added: {reservation.ReservationId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error adding reservation: {ex.Message}");
            }
        }

        // ✅ **ADD FEEDBACK TO DATABASE**
        public void AddFeedback(Feedback feedback)
        {
            try
            {
                if (feedback == null || string.IsNullOrWhiteSpace(feedback.CustomerEmail))
                {
                    Console.WriteLine("❌ Invalid feedback. Skipping insertion.");
                    return;
                }

                _feedbackCollection.InsertOne(feedback);
                Console.WriteLine($"✅ Feedback Added for {feedback.CustomerEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error adding feedback: {ex.Message}");
            }
        }

        // ✅ **GET FEEDBACKS BY EMAIL**
        public List<Feedback> GetFeedbacksByEmail(string email)
        {
            try
            {
                return _feedbackCollection.Find(f => f.CustomerEmail == email).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error retrieving feedbacks: {ex.Message}");
                return new List<Feedback>();
            }
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                return _userCollection.Find(u => u.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error retrieving user: {ex.Message}");
                return null;
            }
        }

        public List<Reservation> GetReservations() => _reservationCollection.Find(_ => true).ToList();
    }
}
