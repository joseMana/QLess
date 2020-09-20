using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLess.Model
{
    public partial class TransportCard : IdentityUser<int, CardLogin, CardRole, CardClaim>, IValidatableObject
    {
        public override int Id => TransportCardId;

        public override string UserName
        {
            get { return TransportCardNumber; }
            set { TransportCardNumber = value; }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            //validation should Match Class and use RegEx -- not enough time
            if (IsPWD == true)
            {
                if (PWDNumber[4] != '-' || PWDNumber[9] != '-' || PWDNumber.Length != 14)
                {
                    results.Add(new ValidationResult("PWD Number should follow the format ####-####-####.", new[] { "Error" }));
                }
            }
            if (IsSeniorCitizen == true)
            {
                if (SeniorCitizenNumber[3] != '-')
                {
                    results.Add(new ValidationResult("Senior Citizen Card Nuber should follow the format ###-####-####.", new[] { "Error" }));
                }
            }

            return results;
        }
    }
    public class CardLogin : IdentityUserLogin<int> { }

    public class CardRole : IdentityUserRole<int> { }

    public class CardClaim : IdentityUserClaim<int> { }
}
