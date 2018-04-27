using System.Web.Mvc;

namespace Blogbaster.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Landing");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Forbidden403()
        {
            Response.StatusCode = 403;
            return View("Error403");
        }
    }
}