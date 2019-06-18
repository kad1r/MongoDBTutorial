using Data;
using Model.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbTutorial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MongoDbTutorial.Controllers
{
    public class UserController : Controller
    {
        private readonly MongoDbContext<User> _userRepository;

        public UserController()
        {
            _userRepository = new MongoDbContext<User>();
        }

        public async Task<ActionResult> Index()
        {
            var vm = new UserVM
            {
                List = _userRepository.collection
                    .AsQueryable()
                    .Where(x => x.IsActive)
                    .ToList()
            };

            return View(vm);
        }

        public async Task<ActionResult> Form(string id)
        {
            var vm = new UserVM();

            if (!string.IsNullOrWhiteSpace(id))
            {
                var objectId = new ObjectId(id);
                var user = await _userRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();

                vm.User = user;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Form(string id, User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        var existing = await _userRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();
                        var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
                        var model = Builders<User>.Update
                            .Set("FullName", user.FullName)
                            .Set("Email", user.Email)
                            .Set("UserName", user.UserName)
                            .Set("Password", user.Password)
                            .Set("IsActive", existing.IsActive)
                            .Set("IsDeleted", existing.IsDeleted)
                            .Set("IsAdmin", user.IsAdmin)
                            .Set("DateInserted", existing.DateInserted)
                            .Set("DateUpdated", DateTime.Now);

                        await _userRepository.collection.UpdateOneAsync(filter, model);
                        TempData["process_result"] = "Update is ok";

                        return RedirectToAction("form");
                    }
                    else
                    {
                        user.DateInserted = DateTime.Now;
                        user.IsActive = true;
                        user.IsDeleted = false;

                        await _userRepository.collection.InsertOneAsync(user);
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
                         * await _userRepository.collection.DeleteOneAsync(Builders<User>.Filter.Eq("_id", ObjectId.Parse(id)));
                         * */

                        var existing = await _userRepository.collection.Find(x => x.Id == id).FirstOrDefaultAsync();
                        var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
                        var model = Builders<User>.Update
                            .Set("IsDeleted", true)
                            .Set("DateUpdated", DateTime.Now);
                        await _userRepository.collection.UpdateOneAsync(filter, model);
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
    }
}
