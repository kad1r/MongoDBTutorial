using Model.Model;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MongoDbTutorial.Models
{
	public class CategoryVM
	{
		public Category Category { get; set; }
		public IAsyncCursor<Category> CategoryModel { get; set; }
		public IEnumerable<Category> List { get; set; }
	}
}
