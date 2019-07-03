using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Model.Model
{
	public class User
	{
		[BsonId]
		public ObjectId Id { get; set; }
		/// <summary>
		/// [BsonElement="FullName"]
		/// Don't change BsonElement attribute if you have records on the document. Because it represents column name on the document.
		/// If you change it to for ex "Full Name" then document field name will be "Full Name", not "FullName".
		/// For instance if you have mssql, mysql or etc and you are also using mongodb then you can use BsonElement to change document field name.
		/// </summary>
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
