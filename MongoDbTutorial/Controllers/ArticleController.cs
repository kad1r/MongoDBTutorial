using Data;
using Model.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbTutorial.Helpers;
using MongoDbTutorial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MongoDbTutorial.Controllers
{
	[CustomAuthorize]
	public class ArticleController : BaseController
	{
		private readonly MongoDbContext<Article> _articleRepository;
		private readonly MongoDbContext<Category> _categoryRepository;
		private readonly MongoDbContext<User> _userRepository;

		public ArticleController()
		{
			_articleRepository = new MongoDbContext<Article>();
			_categoryRepository = new MongoDbContext<Category>();
			_userRepository = new MongoDbContext<User>();
		}

		public async Task<ActionResult> Index()
		{
			var query = from articles in _articleRepository.collection.AsQueryable()
						join user in _userRepository.collection.AsQueryable() on articles.CreatedUserId equals user.Id
						join categories in _categoryRepository.collection.AsQueryable() on articles.CategoryId equals categories.Id into joinedCategories
						select new Article
						{
							Categories = joinedCategories,
							CategoryId = articles.CategoryId,
							CreatedUserId = user.Id,
							DateInserted = articles.DateInserted,
							DatePublished = articles.DatePublished,
							DateUpdated = articles.DateUpdated,
							Description = articles.Description,
							Heading = articles.Heading,
							Id = articles.Id,
							IsActive = articles.IsActive,
							IsDeleted = articles.IsDeleted,
							ReadCount = articles.ReadCount,
							SeoDescription = articles.SeoDescription,
							Summary = articles.Summary,
							TagList = articles.TagList,
							CreatedUser = user
						};

			var vm = new ArticleVM
			{
				List = query.ToList()
			};

			return View(vm);
		}

		public async Task<ActionResult> Form(string id)
		{
			var vm = new ArticleVM
			{
				Categories = _categoryRepository.collection.AsQueryable()
					.Where(x => x.IsActive && !x.IsDeleted)
					.Select(x => new SelectListItem { Text = x.Heading, Value = x.Id })
					.ToList()
			};

			if (!string.IsNullOrWhiteSpace(id))
			{
				var Article = await _articleRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();

				vm.Article = Article;
			}

			return View(vm);
		}

		[HttpPost]
		public async Task<ActionResult> Form(string id, ArticleVM vm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(id))
					{
						var existing = await _articleRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();
						var filter = Builders<Article>.Filter.Eq("_id", ObjectId.Parse(id));
						var model = Builders<Article>.Update
							.Set("CategoryId", vm.Article.CategoryId)
							//.Set("Tags", vm.Article.TagList.ToString().Split(',').ToList())
							.Set("Heading", vm.Article.Heading)
							.Set("Summary", vm.Article.Summary)
							.Set("Description", vm.Article.Description)
							.Set("SeoDescription", vm.Article.SeoDescription)
							.Set("DatePublished", vm.Article.DatePublished)
							.Set("IsActive", existing.IsActive)
							.Set("IsDeleted", existing.IsDeleted)
							.Set("DateInserted", existing.DateInserted)
							.Set("DateUpdated", DateTime.Now)
							.Set("CreatedUserId", User.Id);

						await _articleRepository.collection.UpdateOneAsync(filter, model);
						TempData["process_result"] = "Update is ok";

						return RedirectToAction("index");
					}
					else
					{
						//vm.Article.Tags = vm.Article.TagList.ToString().Split(',').ToList();
						vm.Article.CreatedUserId = User.Id;
						vm.Article.ReadCount = 1;
						vm.Article.DateInserted = DateTime.Now;
						vm.Article.IsDeleted = false;

						await _articleRepository.collection.InsertOneAsync(vm.Article);
						TempData["process_result"] = "Insert is ok";

						return RedirectToAction("form");
					}
				}
				catch (Exception ex)
				{
					var msg = ex.Message;
					TempData["process_result"] = "Error occured: " + msg;

					return RedirectToAction("form");
				}
			}
			else
			{
				return RedirectToAction("index");
			}
		}

		[HttpPost]
		public async Task<JsonResult> Delete(List<string> ids)
		{
			var result = new JsResult();
			var errors = new List<string>();

			if (Request.IsAjaxRequest())
			{
				foreach (var id in ids)
				{
					try
					{
						/*
                         * we actually should not delete record from database
                         * we just need to update delete column
                         * await _articleRepository.collection.DeleteOneAsync(Builders<Article>.Filter.Eq("_id", ObjectId.Parse(id)));
                         * */

						var existing = await _articleRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();
						var filter = Builders<Article>.Filter.Eq("_id", ObjectId.Parse(id));
						var model = Builders<Article>.Update
							.Set("IsActive", false)
							.Set("IsDeleted", true)
							.Set("DateUpdated", DateTime.Now);
						await _articleRepository.collection.UpdateOneAsync(filter, model);
					}
					catch (Exception ex)
					{
						var msg = ex.Message;
						result.HasError = true;
						errors.Add("Id: " + id + " couldn't deleted.");
					}
				}
			}

			result.ErrorMessage = result.HasError ? "Some records could not deleted!" : "Deletion is ok.";
			result.ErrorList = errors;

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		protected async Task CreateIndexOnCollection()
		{
			var notificationLogBuilder = Builders<Article>.IndexKeys;
			var indexModel = new CreateIndexModel<Article>(notificationLogBuilder.Ascending(x => x.Heading));

			await _articleRepository.collection.Indexes.CreateOneAsync(indexModel);
		}
	}
}
