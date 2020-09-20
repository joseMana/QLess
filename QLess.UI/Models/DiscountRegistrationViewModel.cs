using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLess.UI.Models
{
    public class DiscountRegistrationViewModel
    {
        [DisplayName("Are you a Senior Citizen?")]
        public bool IsSeniorCitizen { get; set; }
        
        [DisplayName("Are you a PWD?")]
        public bool IsPWD { get; set; }

        //[RegularExpression(@"/^\d{3}-?\d{4}-?\d{4}-?")]
        [DisplayName("Senior Citizen Control Number")]
        public string SeniorCitizenControlNumber { get; set; }

        //[RegularExpression(@"/^\d{4}-?\d{4}-?\d{4}-?")]
        [DisplayName("PWD ID")]
        public string PWDNumber { get; set; }
    }
}