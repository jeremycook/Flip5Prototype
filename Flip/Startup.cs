using System;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using BrockAllen.MembershipReboot.Owin;
using Flip.Areas.Core;
using Flip.Areas.UserAccount;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Flip.Startup))]

namespace Flip
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // Autofac modules:
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new UserAccountModule(MembershipRebootOwinConstants.AuthenticationType, app));

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // OWIN MVC setup:
            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}
