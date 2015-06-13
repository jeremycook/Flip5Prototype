using System.Web.Mvc;

namespace Flip.Areas.Core
{
    public class CoreAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Core";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Core_default",
                "Core/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { GetType().Namespace + ".Controllers" }
            );
        }
    }
}