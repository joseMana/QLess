﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLess.UI
{
    public class HomeViewModel
    {
        [Display("Transport Card Number")]
        public string TransportCardNumber { get; set; }
    }
}