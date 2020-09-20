using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLess.UI.Models
{
    public class HomeViewModel
    {
        public string TransportCardNumber { get; set; }
        public string TransportCardType { get; set; }
        public decimal CurrentLoad { get; set; }

    }
}