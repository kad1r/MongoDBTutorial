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
	public class CategoryController : BaseController
	{
		private readonly MongoDbContext<Category> _categoryRepository;

		public CategoryController()
		{
			_categoryRepository = new MongoDbContext<Category>();
		}

		public async Task<ActionResult> Index()
		{
			var vm = new CategoryVM
			{
				List = await _categoryRepository.collection
					.AsQueryable()
					.ToListAsync()
			};

			return View(vm);
		}

		public async Task<ActionResult> Form(string id)
		{
			var vm = new CategoryVM();

			if (!string.IsNullOrWhiteSpace(id))
			{
				vm.Category = await _categoryRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();
			}

			return View(vm);
		}

		[HttpPost]
		public async Task<ActionResult> Form(string id, CategoryVM vm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(id))
					{
						var existing = await _categoryRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();
						var filter = Builders<Category>.Filter.Eq("_id", ObjectId.Parse(id));
						var model = Builders<Category>.Update
							.Set("Heading", vm.Category.Heading)
							.Set("Summary", vm.Category.Summary)
							.Set("Description", vm.Category.Description)
							.Set("Sequence", vm.Category.Sequence)
							.Set("IsActive", vm.Category.IsActive)
							.Set("IsDeleted", existing.IsDeleted)
							.Set("DateInserted", existing.DateInserted)
							.Set("DateUpdated", DateTime.Now);

						await _categoryRepository.collection.UpdateOneAsync(filter, model);
						TempData["process_result"] = "Update is ok";

						return RedirectToAction("index");
					}
					else
					{
						vm.Category.DateInserted = DateTime.Now;
						vm.Category.IsDeleted = false;

						await _categoryRepository.collection.InsertOneAsync(vm.Category);
						TempData["process_result"] = "Insert is ok";

						return RedirectToAction("index");
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
						var existing = await _categoryRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();
						var filter = Builders<Category>.Filter.Eq("_id", ObjectId.Parse(id));
						var model = Builders<Category>.Update
							.Set("IsActive", false)
							.Set("IsDeleted", true)
							.Set("DateUpdated", DateTime.Now);

						await _categoryRepository.collection.UpdateOneAsync(filter, model);
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
			var notificationLogBuilder = Builders<Category>.IndexKeys;
			var indexModel = new CreateIndexModel<Category>(notificationLogBuilder.Ascending(x => x.Heading));

			await _categoryRepository.collection.Indexes.CreateOneAsync(indexModel);
		}
	}
}
