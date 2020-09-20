using QLess.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLess.Model;

namespace QLess.UI.Controllers
{
    [Authorize]
    public class HomeController : QLessController
    {
        public ActionResult Index()
        {
            HomeViewModel viewModel;
            using (var db = new QLessEntities())
            {
                var transportCard = db.TransportCards.FirstOrDefault(x => x.TransportCardId == TransportCardId);

                var transportRoleName = transportCard.TransportCardRole.TransportCardRoleName;
                var currentLoad = transportCard.CurrentLoad;

                viewModel = new HomeViewModel
                {
                    TransportCardType = transportRoleName,
                    CurrentLoad = (decimal)currentLoad
                };
            }
            return View(viewModel);
        }
    }
}