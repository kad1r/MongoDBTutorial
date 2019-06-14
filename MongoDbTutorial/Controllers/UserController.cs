using Data;
using Model.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbTutorial.Models;
using System;
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
                var user = await _userRepository.collection
                    .FindAsync(x => x.Id == objectId);

                vm.UserModel = user;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Form(string id, User user)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
                    var model = Builders<User>.Update
                        .Set("FullName", user.FullName)
                        .Set("Email", user.Email)
                        .Set("UserName", user.UserName)
                        .Set("Password", user.Password)
                        .Set("IsActive", user.IsActive)
                        .Set("IsDeleted", user.IsDeleted)
                        .Set("IsAdmin", user.IsAdmin)
                        .Set("DateInserted", user.DateInserted)
                        .Set("DateUpdated", DateTime.Now);
                    var result = _userRepository.collection.UpdateOneAsync(filter, model);

                    return RedirectToAction("index");
                }
                else
                {
                    user.DateInserted = DateTime.Now;
                    user.IsActive = true;
                    user.IsDeleted = false;

                    await _userRepository.collection.InsertOneAsync(user);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _userRepository.collection
                    .DeleteOneAsync(Builders<User>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View("index");
            }
        }
    }
}
