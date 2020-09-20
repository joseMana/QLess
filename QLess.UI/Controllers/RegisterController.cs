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
    public class RegisterController : QLessController
    {
        public ActionResult RegisterDiscounted()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterDiscounted(DiscountRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new QLessEntities())
                    {
                        var transportCard = db.TransportCards.FirstOrDefault(x => x.TransportCardId == TransportCardId);

                        transportCard.IsPWD = model.IsPWD;
                        transportCard.IsSeniorCitizen = model.IsSeniorCitizen;
                        if(model.IsPWD)
                            transportCard.IsSeniorCitizen = model.IsSeniorCitizen;
                        if(model.IsSeniorCitizen)
                            transportCard.SeniorCitizenNumber = model.SeniorCitizenControlNumber;

                        await db.SaveChangesAsync();
                    }
                }
                catch (DbEntityValidationException dbEntityEx)
                {
                    foreach (DbEntityValidationResult ex in dbEntityEx.EntityValidationErrors)
                        foreach (DbValidationError validationError in ex.ValidationErrors)
                            ModelState.AddModelError("", $"{validationError.PropertyName} - {validationError.ErrorMessage}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.ToString());
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}