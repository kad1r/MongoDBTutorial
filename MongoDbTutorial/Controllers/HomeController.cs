using MongoDbTutorial.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MongoDbTutorial.Controllers
{
	[CustomAuthorize]
	public class HomeController : BaseController
	{
		public HomeController()
		{
		}

		public async Task<ActionResult> Index()
		{
			return View();
		}
	}
}
