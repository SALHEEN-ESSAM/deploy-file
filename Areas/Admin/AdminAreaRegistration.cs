using System.Web.Mvc;

namespace Amana.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "{language}/Admin/{controller}/{action}/{id}",
                new { action = "Index", language = "ar", id = UrlParameter.Optional },
                namespaces: new[] { "Amana.Areas.Admin.Controllers" }
            );
        }
    }
}