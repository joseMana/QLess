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
            
            TravelViewModel viewModel = new TravelViewModel()
            {
                MRT2Stations = GetMRT2List()
            };
            return View(viewModel);
        }

        public async Task<JsonResult> AjaxTravelToStation(string entry, string exit)
        {
            using (var db = new QLessEntities())
            {
                var item = db.TransportCards.FirstOrDefault(x => x.TransportCardId == TransportCardId);

                decimal priceOfTravel = db.getMRT2TravelPriceFromPointAtoPointB(entry, exit);
                decimal discount = 0.0M;
                if (item.TodaysNumberOfTravel <= 4)
                {
                    discount = (decimal)item.TransportCardRole.TransportCardRoleDiscount;
                }
                else
                    discount = 0.0M;

                decimal priceToDeduct = priceOfTravel - (priceOfTravel * discount);


                item.CurrentLoad = (int)priceToDeduct;

                item.TodaysNumberOfTravel = item.TodaysNumberOfTravel + 1;

                await db.SaveChangesAsync();

                return Json(new ReturnValue { CurrentLoad = (int)item.CurrentLoad , DeductedAmount = (int)priceToDeduct});
            }
        }

        private IEnumerable<SelectListItem> GetMRT2List()
        {
            List<string> MRT2Stations = new List<string>
            {
                "RECTO",
                "LEGARDA",
                "PUREZA",
                "VMAPA",
                "JRUIZ",
                "GILMORE",
                "BETTY",
                "CUBAO",
                "ANONAS",
                "KATIPUNAN",
                "SANTOLAN"
            };

            IList<SelectListItem> mrt2List = new List<SelectListItem>();
            foreach (string station in MRT2Stations)
            {
                mrt2List.Add(new SelectListItem() { Value = station, Text = station, Selected = false });
            }

            return mrt2List;
        }

        private class ReturnValue
        {
            public int CurrentLoad { get; set; }
            public int DeductedAmount { get; set; }
        }
    }

}