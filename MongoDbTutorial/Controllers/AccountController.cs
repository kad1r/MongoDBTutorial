using Data;
using Model.Model;
using MongoDB.Driver;
using MongoDbTutorial.Helpers;
using MongoDbTutorial.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MongoDbTutorial.Controllers
{
	public class AccountController : Controller
	{
		private readonly MongoDbContext<User> _userRepository;

		public AccountController()
		{
			_userRepository = new MongoDbContext<User>();
		}

		[HttpGet, AllowAnonymous]
		public ActionResult Index()
		{
			if (HttpContext.Request.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<ActionResult> Index(LoginVM vm)
		{
			if (ModelState.IsValid)
			{
				var user = await _userRepository.collection.Find(x => x.Email == vm.Email).FirstOrDefaultAsync();

				if (user != null && user.IsActive && !user.IsDeleted && user.Password == PasswordHelper.HashPassword(vm.Password))
				{
					var authUser = new AuthUser
					{
						Id = user.Id,
						Email = user.Email,
						FullName = user.FullName,
						IsActive = user.IsActive,
						IsAdmin = user.IsAdmin,
						IsDeleted = user.IsDeleted,
						LastLoginDate = DateTime.Now,
						UserName = user.UserName
					};

					var authUserData = JsonConvert.SerializeObject(authUser);
					var ticket = new FormsAuthenticationTicket(999, authUser.Email + "_" + authUser.Id, DateTime.Now, DateTime.Now.AddHours(3), true, authUserData);
					var encrptedTicket = FormsAuthentication.Encrypt(ticket);
					var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrptedTicket)
					{
						Expires = DateTime.Now.AddHours(3)
					};

					Response.Cookies.Add(authCookie);
					//FormsAuthentication.SetAuthCookie(user.Email, false);

					// TODO
					// Add code block to logout user if someone logged in from different session or browser
					// It's for the security reasons

					return RedirectToAction("Index", "Home");
				}
				else if (user.Password != vm.Password)
				{
					ModelState.AddModelError("", "Your password is wrong.");
				}
				else
				{
					ModelState.AddModelError("", "This account is not registered.");
				}
			}

			return View(vm);
		}

		public ActionResult LogOut()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("Index", "Account");
		}
	}
}
