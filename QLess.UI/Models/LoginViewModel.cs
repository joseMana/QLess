using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLess.UI.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required]
        [Display(Name = "Transport Card Number")]
        [RegularExpression(@"[^@]+", ErrorMessage = "Please enter your username, not an email address.")]
        public string TransportCardNumber { get; set; }
    }
}