using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLess.UI.Models
{
    public class TravelViewModel
    {
        public string Entry { get; set; }
        public string Exit { get; set; }
        public System.Collections.Generic.IEnumerable<System.Web.Mvc.SelectListItem> MRT2Stations { get; set; }
    }
}