using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudWars.Game.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Inicio()
        {

            return PartialView();
        }


        public ActionResult ComoJugar()
        {
            return PartialView();
        }


        public ActionResult Bitacora()
        {
            return PartialView();
        }


        public ActionResult Tecnologia()
        {
            return PartialView();
        }



        public ActionResult About()
        {
            return PartialView("Tecnologia");
        }
    }
}
