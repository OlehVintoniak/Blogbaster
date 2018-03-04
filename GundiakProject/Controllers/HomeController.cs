using System.Linq;
using System.Web.Mvc;
using GundiakProject.Enums;
using GundiakProject.Models;

namespace GundiakProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Articles = db.Articles
                .OrderBy(a => a.DateCreated)
                .Where(s => s.Status == Status.Published);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult General()
        {
            return View();
        }
    }
}