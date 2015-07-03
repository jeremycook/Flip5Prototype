using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flip.Areas.Core
{
    public class FlipRazorViewEngine : RazorViewEngine
    {
        public FlipRazorViewEngine()
        {
            AreaViewLocationFormats = new[]
            {
                "~/Areas/App/Views/{1}/{0}.cshtml",
                "~/Areas/App/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/Core/Views/{1}/{0}.cshtml",
                "~/Areas/Core/Views/Shared/{0}.cshtml",
            };
            AreaMasterLocationFormats = new[]
            {
                "~/Areas/App/Views/{1}/{0}.cshtml",
                "~/Areas/App/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/Core/Views/{1}/{0}.cshtml",
                "~/Areas/Core/Views/Shared/{0}.cshtml",
            };
            AreaPartialViewLocationFormats = new[]
            {
                "~/Areas/App/Views/{1}/{0}.cshtml",
                "~/Areas/App/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/Core/Views/{1}/{0}.cshtml",
                "~/Areas/Core/Views/Shared/{0}.cshtml",
            };

            ViewLocationFormats = new[]
            {
                "~/Areas/App/Views/{1}/{0}.cshtml",
                "~/Areas/App/Views/Shared/{0}.cshtml",
                "~/Areas/Core/Views/{1}/{0}.cshtml",
                "~/Areas/Core/Views/Shared/{0}.cshtml",
            };
            MasterLocationFormats = new[]
            {
                "~/Areas/App/Views/{1}/{0}.cshtml",
                "~/Areas/App/Views/Shared/{0}.cshtml",
                "~/Areas/Core/Views/{1}/{0}.cshtml",
                "~/Areas/Core/Views/Shared/{0}.cshtml",
            };
            PartialViewLocationFormats = new[]
            {
                "~/Areas/App/Views/{1}/{0}.cshtml",
                "~/Areas/App/Views/Shared/{0}.cshtml",
                "~/Areas/Core/Views/{1}/{0}.cshtml",
                "~/Areas/Core/Views/Shared/{0}.cshtml",
            };

            FileExtensions = new[]
            {
                "cshtml",
            };
        }
    }
}