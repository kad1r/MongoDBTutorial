using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Model.Model
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string Heading { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
