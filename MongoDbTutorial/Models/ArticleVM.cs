using Model.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MongoDbTutorial.Models
{
	public class ArticleVM
	{
		public Article Article { get; set; }
		public IAsyncCursor<Article> ArticleModel { get; set; }
		public IEnumerable<Article> List { get; set; }
		public IEnumerable<SelectListItem> Categories { get; set; }
	}
}
