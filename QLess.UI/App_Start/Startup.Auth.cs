using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLess.Model;
using QLess.UI.Controllers;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;

namespace QLess.UI
{
    public partial class Startup
    {
        public const string LOGIN_PATH = "/Account/Login";

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(QLessEntities.Create);
            app.CreatePerOwinContext<QLessTransportCardManager>(QLessTransportCardManager.Create);
            app.CreatePerOwinContext<QLessTransportCardSignInManager>(QLessTransportCardSignInManager.Create);
            app.CreatePerOwinContext<QLessTransportCardRoleManager>(QLessTransportCardRoleManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(LOGIN_PATH)
            });
        }
    }
}