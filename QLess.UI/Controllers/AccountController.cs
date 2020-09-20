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
using System.Data.Entity;

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

        [AllowAnonymous]
        public ActionResult CreateCard()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCard(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new TransportCard { UserName = model.Email, Email = "QLessUser1@QLess.com", TransportCardRoleId = 1, LastUsedDate = DateTime.Now };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                 
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
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
            : base(userManager, authenticationManager)
        {
        }
        public async Task<SignInStatus> PasswordSignInAsync(string cardNumber)
        {
            SignInStatus result = await base.PasswordSignInAsync(cardNumber, Constants.TransportCardPassword, false, false);
            return result;
        }
        public static QLessTransportCardSignInManager Create(IdentityFactoryOptions<QLessTransportCardSignInManager> options, IOwinContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            return new QLessTransportCardSignInManager(context.GetUserManager<QLessTransportCardManager>(), context.Authentication);
        }
    }
    public class QLessTransportCardRoleManager : RoleManager<TransportCardRole>
    {
        public QLessTransportCardRoleManager(IRoleStore<TransportCardRole> store)
            : base(store)
        {
        }
        public static QLessTransportCardRoleManager Create(IdentityFactoryOptions<QLessTransportCardRoleManager> options, IOwinContext context)
        {
            //context.Get<EngageEntities>()
            IRoleStore<TransportCardRole> roleStore = new TransportCardRoleStore(context);
            return new QLessTransportCardRoleManager(roleStore);
        }
    }
    public sealed class TransportCardStore : IUserLockoutStore<TransportCard, int>, IUserPasswordStore<TransportCard, int>, IUserEmailStore<TransportCard, int>, IUserLoginStore<TransportCard, int>, IUserPhoneNumberStore<TransportCard, int>, IUserTwoFactorStore<TransportCard, int>, IUserRoleStore<TransportCard, int> //, IUserRoleStore<TransportCard>, IUserClaimStore<TransportCard>, 
    {
        private readonly QLessEntities _db;

        public TransportCardStore(QLessEntities db)
        {
            _db = db;
        }

        #region IUserStore

        public async Task CreateAsync(TransportCard card)
        {
            _db.TransportCards.Add(card);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TransportCard card)
        {
            _db.Entry(card).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(TransportCard card)
        {
            TransportCard transportCard = await _db.TransportCards.FindAsync(card.Id);
            if (transportCard != null) _db.TransportCards.Remove(transportCard);
            await _db.SaveChangesAsync();

        }

        public async Task<TransportCard> FindByIdAsync(int userId)
        {
            return await _db.TransportCards.FindAsync(userId);
        }

        public async Task<TransportCard> FindByNameAsync(string userName)
        {
            return await _db.TransportCards.FirstOrDefaultAsync(e => e.TransportCardNumber == userName);
        }

        #endregion

        #region IUserLockoutStore

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(TransportCard user)
        {
            return await Task.FromResult(false ? DateTimeOffset.Now.AddDays(1) : DateTimeOffset.Now.AddDays(-1));
        }

        public async Task SetLockoutEndDateAsync(TransportCard user, DateTimeOffset lockoutEnd)
        {
        }

        public async Task<int> IncrementAccessFailedCountAsync(TransportCard user)
        {
            return 0;
        }

        public async Task ResetAccessFailedCountAsync(TransportCard user)
        {
        }

        public async Task<int> GetAccessFailedCountAsync(TransportCard user)
        {
            return await Task.FromResult(0);
        }

        public async Task<bool> GetLockoutEnabledAsync(TransportCard user)
        {
            return await Task.FromResult(false);
        }

        public async Task SetLockoutEnabledAsync(TransportCard user, bool enabled)
        {
        }

        #endregion

        #region IUserPasswordStore

        public async Task SetPasswordHashAsync(TransportCard user, string passwordHash)
        {
            if (String.IsNullOrEmpty(passwordHash))
                throw new ArgumentNullException(passwordHash, "Value cannot be null or empty");
            byte[] password = Convert.FromBase64String(passwordHash);
            user.Password = password;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the given user's password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<string> GetPasswordHashAsync(TransportCard user)
        {
            string passwordHash = "";
            if (user.Password != null)
                passwordHash = Convert.ToBase64String(user.Password);
            return await Task.FromResult(passwordHash);
        }

        public async Task<bool> HasPasswordAsync(TransportCard user)
        {
            return await Task.FromResult(false);
        }

        #endregion

        #region IUserEmailStore

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task SetEmailAsync(TransportCard user, string email)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            user.Email = email;
        }

        public async Task<string> GetEmailAsync(TransportCard user)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(TransportCard user)
        {
            return await Task.FromResult(true);
        }

        public async Task SetEmailConfirmedAsync(TransportCard user, bool confirmed)
        {
        }

        public async Task<TransportCard> FindByEmailAsync(string email)
        {
            TransportCard result = await _db.TransportCards.FirstOrDefaultAsync();
            return await Task.FromResult(result);

        }

        #endregion

        #region IUserTwoFactoreStore - not implemented

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task SetTwoFactorEnabledAsync(TransportCard user, bool enabled)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // do nothing
        }

        public async Task<bool> GetTwoFactorEnabledAsync(TransportCard user)
        {
            return await Task.FromResult(false);
        }

        #endregion

        #region IUserLoginStore - not implemented

        public Task AddLoginAsync(TransportCard user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(TransportCard user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TransportCard user)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }

        public Task<TransportCard> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserPhoneNumberStore - not implemented
        public Task SetPhoneNumberAsync(TransportCard user, string phoneNumber)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> GetPhoneNumberAsync(TransportCard user)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TransportCard user)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(TransportCard user, bool confirmed)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDisposable
        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                // free managed objects
                _db?.Dispose();
            }
            // free unmanaged objects
            _disposed = true;
        }
        #endregion

        public Task AddToRoleAsync(TransportCard user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(TransportCard user, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(TransportCard user)
        {
            IList<string> roles = new List<string>();
            
            using(var q = new QLessEntities())
            {
                roles.Add(q.TransportCardRoles
                    .FirstOrDefault(x => x.TransportCardRoleId == user.TransportCardRoleId).TransportCardRoleName);
            }
            
            return await Task.FromResult(roles);
        }

        public async Task<bool> IsInRoleAsync(TransportCard user, string roleName)
        {
            return (await GetRolesAsync(user)).Contains(roleName);
        }
    } 
}