using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Model.Model
{
	public class Article
	{
		[BsonId, BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string CategoryId { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string CreatedUserId { get; set; }

		public string Heading { get; set; }
		public string Summary { get; set; }

		[AllowHtml]
		public string Description { get; set; }

		public string SeoDescription { get; set; }
		public int ReadCount { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime DatePublished { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime DateInserted { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime? DateUpdated { get; set; }

		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public string TagList { get; set; }
		public User CreatedUser { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Tag> Tags { get; set; }
	}
}
