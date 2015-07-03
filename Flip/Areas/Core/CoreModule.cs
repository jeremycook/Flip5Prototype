using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;

namespace Flip.Areas.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            ModelMetadataProviders.Current = new ConventionalModelMetadataProvider();
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