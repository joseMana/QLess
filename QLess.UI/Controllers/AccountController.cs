using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using QLess.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using QLess.Model;

namespace QLess.UI.Controllers
{
    public class AccountController : Controller
    {
        private QLessTransportCardSignInManager _signInManager;
        private QLessTransportCardManager _userManager;
        private QLessTransportCardRoleManager _roleManager;

        public AccountController()
        {
        }
        public AccountController(QLessTransportCardManager userManager, QLessTransportCardSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public AccountController(QLessTransportCardManager userManager, QLessTransportCardSignInManager signInManager, QLessTransportCardRoleManager roleManager) : this(userManager, signInManager)
        {
            _roleManager = roleManager;
        }
        public QLessTransportCardSignInManager SignInManager
        {
            get 
            { 
                return _signInManager ?? HttpContext.GetOwinContext().Get<QLessTransportCardSignInManager>(); 
            }
            private set 
            { 
                _signInManager = value; 
            }
        }
        public QLessTransportCardManager UserManager
        {
            get 
            { 
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<QLessTransportCardManager>(); 
            }
            private set 
            { 
                _userManager = value; 
            }
        }
        public QLessTransportCardRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<QLessTransportCardRoleManager>();
            }
            private set 
            { 
                _roleManager = value; 
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);
            SignInStatus result = await SignInManager.PasswordSignInAsync(model.TransportCardNumber);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }
    }

    public class QLessTransportCardRoleManager
    {
    }

    public class QLessTransportCardManager : UserManager<TransportCard, int>
    {
        // ReSharper disable MemberCanBePrivate.Global
        public QLessTransportCardManager(IUserStore<TransportCard, int> store)
            // ReSharper restore MemberCanBePrivate.Global
            : base(store)
        {
        }

        public static QLessTransportCardManager Create(IdentityFactoryOptions<QLessTransportCardManager> options, IOwinContext context)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            QLessEntities db = context.Get<QLessEntities>();
            QLessTransportCardManager manager = new QLessTransportCardManager(new TransportCardStore(db));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<TransportCard, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            
            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<TransportCard, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class QLessTransportCardSignInManager : SignInManager<TransportCard, int>
    {
        public QLessTransportCardSignInManager(QLessTransportCardManager userManager, IAuthenticationManager authenticationManager)
            // ReSharper restore MemberCanBePrivate.Global
            : base(userManager, authenticationManager)
        {
        }
        public Task<SignInStatus> PasswordSignInAsync(string cardNumber)
        {
            throw new NotImplementedException();
        }
    }

    public partial class TransportCard : IdentityUser<int, EmployeeUserLogin, EmployeeUserRole, EmployeeUserClaim>
    {

        
    }

    public class EmployeeUserLogin : IdentityUserLogin<int> { }

    public class EmployeeUserRole : IdentityUserRole<int> { }

    public class EmployeeUserClaim : IdentityUserClaim<int> { }

    public sealed class TransportCardStore : IUserLockoutStore<TransportCard, int>, IUserPasswordStore<TransportCard, int>, IUserEmailStore<TransportCard, int>, IUserLoginStore<TransportCard, int>, IUserPhoneNumberStore<TransportCard, int>, IUserTwoFactorStore<TransportCard, int>, IUserRoleStore<TransportCard, int> //, IUserRoleStore<TransportCard>, IUserClaimStore<TransportCard>, 
    {
        private readonly QLessEntities _db;

        public TransportCardStore(QLessEntities db)
        {
            _db = db;
        }

        public Task AddLoginAsync(TransportCard user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(TransportCard user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<TransportCard> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<TransportCard> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<TransportCard> FindByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<TransportCard> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(TransportCard user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(TransportCard user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(TransportCard user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(TransportCard user, string email)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(TransportCard user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(TransportCard user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(TransportCard user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(TransportCard user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberAsync(TransportCard user, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(TransportCard user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(TransportCard user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }
    } 
}