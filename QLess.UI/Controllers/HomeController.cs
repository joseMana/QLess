using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLess.UI.Controllers
{
    public class HomeController : QLessController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}