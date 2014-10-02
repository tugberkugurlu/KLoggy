using Microsoft.AspNet.Mvc;

namespace KLoggy.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}