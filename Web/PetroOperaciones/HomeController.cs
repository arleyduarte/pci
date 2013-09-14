using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetroOperaciones.Controllers
{
    public class HomeController : Controller
    {

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Bienvenido al sistema de gestión de DO";

            if (User.IsInRole("Admin"))
            {

            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
