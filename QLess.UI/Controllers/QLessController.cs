using QLess.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLess.Model;
using Microsoft.AspNet.Identity;

namespace QLess.UI.Controllers
{
    public abstract class QLessController : Controller
    {
        protected int TransportCardId { get; set; }

        protected IRepository Repository { get; private set; }
    
        protected QLessController()
        {
            TransportCardId = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            Repository = new GenericRepository(new QLessEntities(TransportCardId));
        }

        protected QLessController(IRepository repository, int employeeId)
        {
            TransportCardId = employeeId;
            Repository = repository;
        }

        protected override void Dispose(bool disposing)
        {
            Repository = null;
            base.Dispose(disposing);
        }
    }
}