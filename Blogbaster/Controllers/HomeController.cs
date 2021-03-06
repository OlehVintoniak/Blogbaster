﻿#region

using Blogbaster.Models;
using System.Web.Mvc;

#endregion

namespace Blogbaster.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View("Landing");
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

        public ActionResult Forbidden403()
        {
            Response.StatusCode = 403;
            return View("Error403");
        }
    }
}