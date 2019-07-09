using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.Model
{
	public class Tag
	{
		[BsonId, BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Heading { get; set; }
	}
}
