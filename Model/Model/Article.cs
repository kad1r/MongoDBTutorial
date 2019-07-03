using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Model.Model
{
    public class Article
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string SeoDescription { get; set; }
        public int ReadCount { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public User CreatedUser { get; set; }
        public IList<Category> Categories { get; set; }
    }
}
