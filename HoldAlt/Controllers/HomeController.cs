using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoldAlt.Classes;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace HoldAlt.Controllers
{
    public class HomeController : Controller
    {
        static List<string> lSearchResults = new List<string>();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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



    }

}