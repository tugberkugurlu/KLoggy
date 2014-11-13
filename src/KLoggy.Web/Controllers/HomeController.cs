using Microsoft.AspNet.Mvc;

namespace KLoggy.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}