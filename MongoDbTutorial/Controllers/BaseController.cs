using MongoDbTutorial.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MongoDbTutorial.Controllers
{
	public class BaseController : Controller
	{
		public BaseController()
		{
		}

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
		}

		protected override void OnException(ExceptionContext exceptionContext)
		{
			base.OnException(exceptionContext);

			if (exceptionContext.ExceptionHandled)
			{
				Response.Redirect("~/status/error");
			}
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
		}

		protected virtual new LoggedInUser User
		{
			get { return HttpContext.User as LoggedInUser; }
		}
	}
}
