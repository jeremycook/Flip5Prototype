using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Flip.Areas.Core;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Flip.Startup))]

namespace Flip
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // STANDARD MVC SETUP:
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FlipRazorViewEngine());
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(Startup).Assembly);

            // Run other optional steps, like registering model binders,
            // web abstractions, etc., 

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(Startup).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // OWIN MVC SETUP:

            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            AreaRegistration.RegisterAllAreas();
            // TODO: Specify default page
            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { action = "Index", controller = "Default", area = "App" }
            );
        }

        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
