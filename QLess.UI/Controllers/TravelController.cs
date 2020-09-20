using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using QLess.Model;
using QLess.UI.Models;

namespace QLess.UI.Controllers
{
    [Authorize]
    public class TravelController : QLessController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterDiscounted(DiscountRegistrationViewModel model)
        {
           
        }
    }
}