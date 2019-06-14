using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Model.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
