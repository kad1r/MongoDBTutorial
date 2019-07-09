using Model.Model;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MongoDbTutorial.Helpers
{
	public class CustomAuthorizeAttribute : AuthorizeAttribute
	{
		protected virtual LoggedInUser CurrentUser
		{
			get { return HttpContext.Current.User as LoggedInUser; }
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext.HttpContext.Request.IsAuthenticated)
			{
				var authCookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

				if (authCookie != null)
				{
					var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
					var userData = JsonConvert.DeserializeObject<AuthUser>(authTicket.UserData);

					if (userData != null)
					{
						var loggedInUser = new LoggedInUser(authTicket.Name)
						{
							Email = userData.Email,
							FullName = userData.FullName,
							Id = userData.Id,
							IsActive = userData.IsActive,
							IsAdmin = userData.IsAdmin,
							IsDeleted = userData.IsDeleted,
							UserName = userData.UserName
						};

						HttpContext.Current.User = loggedInUser;
					}
					else
					{
						FormsAuthentication.SignOut();
						filterContext.Result = new RedirectResult("~/Account/Index");
					}
				}
			}
			else
			{
				FormsAuthentication.SignOut();

				if (!filterContext.HttpContext.Request.IsAjaxRequest())
				{
					filterContext.Result = new RedirectResult("~/Account/Index");
				}
				else
				{
					filterContext.HttpContext.Response.AddHeader("REQUIRES_AUTH", "1");
					filterContext.HttpContext.Response.StatusCode = 403;
					filterContext.Result = new JsonResult { };
				}
			}
		}
	}
}
