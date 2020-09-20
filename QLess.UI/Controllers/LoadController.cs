using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using QLess.Model;

namespace QLess.UI.Controllers
{
    [Authorize]
    public class LoadController : QLessController
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> AjaxLoadTransportCard(int loadValue)
        {
            using(var db = new QLessEntities())
            {
                var item = db.TransportCards.FirstOrDefault(x => x.TransportCardId == TransportCardId);
                item.CurrentLoad += loadValue;

                await db.SaveChangesAsync();
            }

            return Json(true);
        }
    }

}