using System.Web.Mvc;

namespace Flip.Areas.CleanBlog
{
    public class CleanBlogAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CleanBlog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CleanBlog_default",
                "CleanBlog/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}