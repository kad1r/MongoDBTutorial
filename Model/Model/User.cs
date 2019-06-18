using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Model.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        //public ObjectId Id { get; set; }
        public string Id { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }

        [BsonElement("IsAdmin")]
        public bool IsAdmin { get; set; }

        [BsonElement("DateInserted")]
        public DateTime DateInserted { get; set; }

        [BsonElement("DateUpdated")]
        public DateTime? DateUpdated { get; set; }

        [BsonElement("IsActive")]
        public bool IsActive { get; set; }

        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
}
