using Data;
using Model.Model;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MongoDbTutorial.Controllers
{
    public class HomeController : Controller
    {
        private readonly MongoDbContext<User> _userRepository;

        public HomeController()
        {
            _userRepository = new MongoDbContext<User>();
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}
